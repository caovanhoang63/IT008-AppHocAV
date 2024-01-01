using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Controls;
using IT008_AppHocAV.Models;
using MimeKit.Encodings;

namespace IT008_AppHocAV.Repositories.DbConnection
{
    public class DictionaryRepository
    {
        private SqlConnection _sqlConnection;

        public DictionaryRepository(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public DictionaryEntry GetDictionaryEntry(string word)
        {
            try
            {
                DictionaryEntry dictionaryEntry =  GetWordDictionaryEntry(word);
                if (dictionaryEntry != null)
                {
                    dictionaryEntry.phonetics = GetPhonetic(dictionaryEntry.id);
                    dictionaryEntry.meanings = GetMeanings(dictionaryEntry.id);
                    foreach (var meaning in dictionaryEntry.meanings)
                    {
                        meaning.partOfSpeech = GetPartOfSpeech(meaning.partOfSpeechId);
                        meaning.definitions = GetDefinitions(meaning.id);
                    }
                    dictionaryEntry.phonetics = GetPhonetic(dictionaryEntry.id);
                    return dictionaryEntry;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        public  DictionaryEntry GetWordDictionaryEntry(string word)
        {
            try
            {
                DictionaryEntry dictionaryEntry = new DictionaryEntry();
                string query = "SELECT * FROM DictionaryEntry WHERE word = @word";
            
                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.AddWithValue("@word", word);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dictionaryEntry.id = (int)reader["id"];
                            dictionaryEntry.word = reader["word"].ToString();
                            dictionaryEntry.phonetic = reader["phonetic"].ToString();
                            return dictionaryEntry;
                        }

                        return null;
                    }                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
            
        }  
        
        public List<Meaning> GetMeanings(int wordId)
        {
            try
            {
                List<Meaning> meanings = new List<Meaning>();
                string query = "SELECT * FROM Meaning WHERE dictionaryEntryId = @wordId";

                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.AddWithValue("@wordId", wordId);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Meaning meaning = new Meaning();
                            meaning.id = (int)reader["id"];
                            meaning.partOfSpeechId = (int)reader["partOfSpeechId"];
                            
                            meaning.synonyms = new List<Text>();

                            foreach (var text in reader["synonyms"].ToString().Split('\n').ToList() )
                            {
                                meaning.synonyms.Add(new Text(text));
                            }

                            meaning.antonyms = new List<Text>();
                            foreach (var text in reader["antonyms"].ToString().Split('\n').ToList())
                            {
                                meaning.antonyms.Add(new Text(text));
                            }
                            meanings.Add(meaning);
                        }

                        return meanings;
                    }                    
                }
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
            
        }
        public List<Definition> GetDefinitions(int meaningId)
        {
            try
            {
                List<Definition> definitions = new List<Definition>();
                string query = "SELECT * FROM Definition WHERE meaningId = @meaningId";
            
                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.AddWithValue("@meaningId", meaningId);
                    _sqlConnection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Definition definition = new Definition();
                            definition.id = (int)reader["id"];
                            definition.definition = reader["definitionText"].ToString();
                            definition.example = reader["example"].ToString();
                            
                            definition.synonyms = new List<Text>();
                            foreach (var text in reader["synonyms"].ToString().Split('\n').ToList() )
                            {
                                definition.synonyms.Add(new Text(text));
                                
                            }
                            definition.antonyms = new List<Text>();
                            foreach (var text in reader["antonyms"].ToString().Split('\n').ToList())
                            {
                                definition.antonyms.Add(new Text(text));
                            }

                            definitions.Add(definition);
                        }

                        return definitions;
                    }                    
                }
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
            
        }
        
        public List<Phonetic> GetPhonetic(int wordId)
        {
            try
            {
                List<Phonetic> phonetics = new List<Phonetic>();
                string query = "SELECT * FROM Phonetic WHERE dictionaryEntryId = @wordId";
                _sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.AddWithValue("@wordId", wordId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Phonetic phonetic = new Phonetic();
                            phonetic.id = (int)reader["id"];
                            phonetic.text = reader["text"].ToString();
                            phonetic.audio = reader["audio"].ToString();
                            phonetics.Add(phonetic);
                        }

                        return phonetics;
                    }                    
                }
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
            
        }

        public string GetPartOfSpeech(int partOfSpeechId)
        {
            try
            {
                _sqlConnection.Close();
                string partOfSpeech = "";
                string query = "SELECT * FROM PartOfSpeech WHERE id = @partOfSpeechId";
                _sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(query,_sqlConnection))
                {
                    command.Parameters.AddWithValue("@partOfSpeechId", partOfSpeechId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            partOfSpeech = reader["name"].ToString();
                        }
                        return partOfSpeech;
                    }                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}