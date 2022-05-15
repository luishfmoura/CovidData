using CovidData.Data;
using CovidData.Models;
using CovidData.Models.Returns;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CovidData.Services
{
    public class CovidCasesService
    {
        private readonly CovidDataContext _context;

        public CovidCasesService(CovidDataContext context)
        {
            _context = context;
        }

        public Response DefaultMessage()
        {
            try
            {
                string data = "Backend Challenge 2021 🏅 -Covid Daily Cases";

                return new Response
                {
                    Data = data,
                    Status = true,
                    Message = "Sucesso!"
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    Data = null,
                    Status = false,
                    Message = e.Message
                };
            }
        }

        public Response GetCount(string dateString)
        {
            try
            {
                DateTime date = Convert.ToDateTime(dateString);

                List<CovidPerLocationVariant> covidLocationVariants = new List<CovidPerLocationVariant>();

                var covidCasesLocation = _context.covidCase.Where(d => d.Date == date).AsEnumerable().GroupBy(l => l.Location).ToList();

                foreach (var location in covidCasesLocation)
                {
                    List<CovidCase> covidCasesVariant = new List<CovidCase>();

                    var variants = location.GroupBy(v => v.Variant).ToList();

                    foreach (var variant in variants)
                    {
                        CovidCase covidCase = new CovidCase()
                        {
                            Variant = variant.Key,
                            Location = location.Key,
                            Date = variant.FirstOrDefault().Date,
                            NumSequences = variant.FirstOrDefault().NumSequences,
                            NumSequencesTotal = variant.FirstOrDefault().NumSequencesTotal,
                            PercSequences = variant.FirstOrDefault().PercSequences
                        };
                        covidCasesVariant.Add(covidCase);
                    }

                    CovidPerLocationVariant covidLocationVariant = new CovidPerLocationVariant()
                    {
                        Location = location.Key,
                        CovidCases = covidCasesVariant
                    };
                    covidLocationVariants.Add(covidLocationVariant);
                }

                if (covidLocationVariants.Count != 0)
                {
                    return new Response
                    {
                        Data = covidLocationVariants,
                        Status = true,
                        Message = "Sucesso!"

                    };
                }
                else
                {
                    return new Response
                    {
                        Data = null,
                        Status = true,
                        Message = "No data found in this date!"
                    };
                }

            }
            catch (Exception e)
            {
                return new Response
                {
                    Data = null,
                    Status = false,
                    Message = e.Message
                };
            }
        }

        public Response GetCumulative(string dateString)
        {
            try
            {
                DateTime date = Convert.ToDateTime(dateString);

                List<CovidPerLocationVariant> covidLocationVariants = new List<CovidPerLocationVariant>();

                var covidCasesLocation = _context.covidCase.Where(d => d.Date == date).AsEnumerable().GroupBy(l => l.Location).ToList();

                foreach (var location in covidCasesLocation)
                {
                    List<CovidCase> covidCasesVariant = new List<CovidCase>();

                    var variants = location.GroupBy(v => v.Variant).ToList();

                    foreach (var variant in variants)
                    {
                        CovidCase covidCase = new CovidCase()
                        {
                            Variant = variant.Key,
                            Location = location.Key,
                            Date = variant.FirstOrDefault().Date,
                            NumSequences = variant.Sum(n => n.NumSequences),
                            NumSequencesTotal = variant.FirstOrDefault().NumSequencesTotal,
                            PercSequences = variant.Sum(n => n.PercSequences)

                        };
                        covidCasesVariant.Add(covidCase);
                    }

                    CovidPerLocationVariant covidLocationVariant = new CovidPerLocationVariant()
                    {
                        Location = location.Key,
                        CovidCases = covidCasesVariant
                    };
                    covidLocationVariants.Add(covidLocationVariant);
                }

                if (covidLocationVariants.Count != 0)
                {
                    return new Response
                    {
                        Data = covidLocationVariants,
                        Status = true,
                        Message = "Sucesso!"
                    };
                }
                else
                {
                    return new Response
                    {
                        Data = null,
                        Status = true,
                        Message = "No data found in this date!"
                    };
                }
            }
            catch (Exception e)
            {
                return new Response
                {
                    Data = null,
                    Status = false,
                    Message = e.Message
                };
            }
        }

        public Response GetDates()
        {
            try
            {
                List<DateTime> dates = _context.covidCase.Select(d => d.Date).Distinct().ToList();

                if (dates.Count != 0)
                {
                    return new Response
                    {
                        Data = dates,
                        Status = true,
                        Message = "Sucesso!"
                    };
                }
                else
                {
                    return new Response
                    {
                        Data = null,
                        Status = true,
                        Message = "No data found!"
                    };
                }
            }
            catch (Exception e)
            {
                return new Response
                {
                    Data = null,
                    Status = false,
                    Message = e.Message
                };
            }
        }

        public Response UploadData(IFormFile file, bool persistData)
        {
            try
            {
                var fileextension = Path.GetExtension(file.FileName);
                var filename = Guid.NewGuid().ToString() + fileextension;
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", filename);
                using (FileStream fs = System.IO.File.Create(filepath))
                {
                    file.CopyTo(fs);
                }
                if (fileextension == ".csv")
                {
                    using (var reader = new StreamReader(filepath))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var records = csv.GetRecords<CovidCaseCsv>().ToList();

                            if (persistData == true)
                            {
                                foreach (var record in records)
                                {

                                    if (string.IsNullOrWhiteSpace(record.location))
                                    {
                                        break;
                                    }

                                    CovidCase covidCase = _context.covidCase
                                        .Where(s => s.Date == record.date && s.Location == record.location && s.Variant == record.variant)
                                        .FirstOrDefault();

                                    if (covidCase == null)
                                    {
                                        covidCase = new CovidCase();
                                    }

                                    covidCase.Date = record.date;
                                    covidCase.Location = record.location;
                                    covidCase.NumSequences = record.num_sequences;
                                    covidCase.NumSequencesTotal = record.num_sequences_total;
                                    covidCase.PercSequences = record.perc_sequences;
                                    covidCase.Variant = record.variant;

                                    if (covidCase.Id == 0)
                                    {
                                        _context.covidCase.Add(covidCase);
                                    }
                                    else
                                    {
                                        _context.covidCase.Update(covidCase);
                                    }
                                }
                            }
                            else
                            {

                                int i = 0;

                                List<CovidCase> temp = new List<CovidCase>();

                                foreach (var record in records)
                                {
                                    CovidCase covidCase = new CovidCase();
                                    covidCase.Date = record.date;
                                    covidCase.Location = record.location;
                                    covidCase.NumSequences = record.num_sequences;
                                    covidCase.NumSequencesTotal = record.num_sequences_total;
                                    covidCase.PercSequences = record.perc_sequences;
                                    covidCase.Variant = record.variant;
                                    temp.Add(covidCase);
                                    i++;
                                }

                                _context.covidCase.AddRange(temp);
                            }

                            _context.SaveChanges();

                            return new Response
                            {
                                Data = null,
                                Status = true,
                                Message = "Data uploaded successfully"
                            };
                        }
                    }
                }
                else
                {
                    return new Response
                    {
                        Data = null,
                        Status = false,
                        Message = "You can only add CSV file",
                    };
                }
            }
            catch (Exception e)
            {
                return new Response
                {
                    Data = null,
                    Status = false,
                    Message = e.Message
                };
            }
        }

        public Response DeleteData()
        {
            try
            {
                var a = _context.covidCase.ToList();
                _context.RemoveRange(a);
                _context.SaveChanges();
                return new Response
                {
                    Data = null,
                    Status = true,
                    Message = "Data deleted from the database"
                };
            }
            catch (Exception e)
            {
                return new Response
                {
                    Data = null,
                    Status = false,
                    Message = e.Message
                };
            }
        }
    }
}
