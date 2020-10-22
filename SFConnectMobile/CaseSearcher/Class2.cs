using Lucene.Net;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Version = Lucene.Net.Util;

namespace CaseSearcher
{
    public class Class2
    {

        public Class2()
        {
            this.CreateIndex();
            this.Search("USA", "", null, null);

            //LuceneSearch.AddUpdateLuceneIndex(SampleDataRepository.GetAll());
            //var new_record = new SampleData { Id = 12, Name = "SomeName", Description = "SomeDescription" };
            //// todo: add record to database...

            //// add record to Lucene search index
            //LuceneSearch.AddUpdateLuceneIndex(new_record);

        }

        public void Search(string searchTerm, string searchField, bool? searchDefault, int? limit)
        {

            List<SampleData> _searchResults;


            _searchResults = (string.IsNullOrEmpty(searchField)
                                ? LuceneSearch.Search(searchTerm)
                                : LuceneSearch.Search(searchTerm, searchField)).ToList();
            if (string.IsNullOrEmpty(searchTerm) && !_searchResults.Any())
                _searchResults = LuceneSearch.GetAllIndexRecords().ToList();


            // limit display number of database records
            var limitDb = limit == null ? 3 : Convert.ToInt32(limit);
            List<SampleData> allSampleData;
            if (limitDb > 0)
            {
                allSampleData = SampleDataRepository.GetAll().ToList().Take(limitDb).ToList();
            }
            else allSampleData = SampleDataRepository.GetAll();


            var AllSampleData = allSampleData;
            var SearchIndexData = _searchResults;

        }

        public void CreateIndex()
        {
            LuceneSearch.AddUpdateLuceneIndex(SampleDataRepository.GetAll());
        }

        public void AddToIndex(SampleData sampleData)
        {
            LuceneSearch.AddUpdateLuceneIndex(sampleData);
        }

        public string ClearIndex()
        {
            if (LuceneSearch.ClearLuceneIndex())
                return "Search index was cleared successfully!";
            else
                return "Index is locked and cannot be cleared, try again later or clear manually!";
        }

        public void ClearIndexRecord(int id)
        {
            LuceneSearch.ClearLuceneIndexRecord(id);
        }

        public void OptimizeIndex()
        {
            LuceneSearch.Optimize();
        }



        public static class LuceneSearch
        {
            

            private static string _luceneDir = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "lucene_index");
            private static FSDirectory _directoryTemp;
            private static FSDirectory _directory
            {
                get
                {
                    if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                    if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                    var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                    if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                    return _directoryTemp;
                }
            }

            static LuceneSearch()
            {
                try
                {
                    //if (!System.IO.Directory.Exists(LuceneSearch._luceneDir))
                    //{
                    //    System.IO.Directory.CreateDirectory(LuceneSearch._luceneDir);
                    //}
                }
                catch (Exception e)
                {
                    var exc = e.Message;
                    throw;
                }
            }

   

            public static IEnumerable<SampleData> GetAllIndexRecords()
            {
                // validate search index
                if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<SampleData>();

                // set up lucene searcher
                var reader = DirectoryReader.Open(_directory);
                var searcher = new IndexSearcher(reader);
                var docs = new List<Document>();
                var term = reader.TermDocs();
                while (term.Next()) docs.Add(searcher.Doc(term.Doc));
                reader.Dispose();
                searcher.Dispose();
                return _mapLuceneToDataList(docs);
            }

            private static void _addToLuceneIndex(SampleData sampleData, IndexWriter writer)
            {
                // remove older index entry
                var searchQuery = new Lucene.Net.Search.TermQuery(new Term("Id", sampleData.Id.ToString()));
                writer.DeleteDocuments(searchQuery);

                // add new index entry
                var doc = new Lucene.Net.Documents.Document();

                // add lucene fields mapped to db fields
                doc.Add(new Field("Id", sampleData.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("Name", sampleData.Name, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("Description", sampleData.Description, Field.Store.YES, Field.Index.ANALYZED));

                // add entry to index
                writer.AddDocument(doc);
            }

            public static void AddUpdateLuceneIndex(IEnumerable<SampleData> sampleDatas)
            {
                // init lucene
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // add data to lucene search index (replaces older entry if any)
                    foreach (var sampleData in sampleDatas) _addToLuceneIndex(sampleData, writer);

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }

            public static void AddUpdateLuceneIndex(SampleData sampleData)
            {
                AddUpdateLuceneIndex(new List<SampleData> { sampleData });
            }

            public static void ClearLuceneIndexRecord(int record_id)
            {
                // init lucene
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entry
                    var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                    writer.DeleteDocuments(searchQuery);

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }

            public static bool ClearLuceneIndex()
            {
                try
                {
                    var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                    using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        // remove older index entries
                        writer.DeleteAll();

                        // close handles
                        analyzer.Close();
                        writer.Dispose();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }

            public static void Optimize()
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    analyzer.Close();
                    writer.Optimize();
                    writer.Dispose();
                }
            }

            private static SampleData _mapLuceneDocumentToData(Document doc)
            {
                return new SampleData
                {
                    Id = Convert.ToInt32(doc.Get("Id")),
                    Name = doc.Get("Name"),
                    Description = doc.Get("Description")
                };
            }

            private static IEnumerable<SampleData> _mapLuceneToDataList(IEnumerable<Document> hits)
            {
                return hits.Select(_mapLuceneDocumentToData).ToList();
            }
            private static IEnumerable<SampleData> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits,
                IndexSearcher searcher)
            {
                return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
            }

            private static Query parseQuery(string searchQuery, QueryParser parser)
            {
                Query query;
                try
                {
                    //You can use FuzzyQuery to lookup misspelled words
                    query = parser.Parse(searchQuery.Trim());
                }
                catch (ParseException)
                {
                    query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
                }
                return query;
            }

            private static IEnumerable<SampleData> _search(string searchQuery, string searchField = "")
            {
                // validation
                if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) return new List<SampleData>();

                // set up lucene searcher
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var hits_limit = 1000;
                    var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                    // search by single field
                    if (!string.IsNullOrEmpty(searchField))
                    {
                        var parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, hits_limit).ScoreDocs;
                        var results = _mapLuceneToDataList(hits, searcher);
                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                    // search by multiple fields (ordered by RELEVANCE)
                    else
                    {
                        var parser = new MultiFieldQueryParser
                            (Version.LUCENE_30, new[] { "Id", "Name", "Description" }, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search
                        (query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                        var results = _mapLuceneToDataList(hits, searcher);
                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                }
            }

            public static IEnumerable<SampleData> Search(string input, string fieldName = "")
            {
                if (string.IsNullOrEmpty(input)) return new List<SampleData>();

                var terms = input.Trim().Replace("-", " ").Split(' ')
                    .Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
                input = string.Join(" ", terms);

                return _search(input, fieldName);
            }

            public static IEnumerable<SampleData> SearchDefault(string input, string fieldName = "")
            {
                return string.IsNullOrEmpty(input) ? new List<SampleData>() : _search(input, fieldName);
            }
        }


        public class SampleData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }




        public static class SampleDataRepository
        {
            public static SampleData Get(int id)
            {
                return GetAll().SingleOrDefault(x => x.Id.Equals(id));
            }
            public static List<SampleData> GetAll()
            {
                return new List<SampleData> {
                    new SampleData {Id = 1, Name = "Belgrad", Description = "City in Serbia"},
                    new SampleData {Id = 2, Name = "Moscow", Description = "City in Russia"},
                    new SampleData {Id = 3, Name = "Chicago", Description = "City in USA"},
                    new SampleData {Id = 4, Name = "Mumbai", Description = "City in India"},
                    new SampleData {Id = 5, Name = "Hong-Kong", Description = "City in Hong-Kong"},
                };
                        }
        }

        
    }
}
