﻿using System.ComponentModel.DataAnnotations;
using Application.Commands;
using Application.Commands.Create.JobTitle;
using Application.Commands.CreateOrUpdate.Employee;
using Application.Interfaces;
using MediatR;

namespace Application;

public class LazyFileParser : IFileParser
{
    private readonly IFormatParser formatParser;

    public LazyFileParser(IFormatParser formatParser)
    {
        this.formatParser = formatParser;
    }
    public async IAsyncEnumerable<List<string[]>> ParseFile(string filepath)
    {
        var parsedLines = new List<string[]>(1000);
        using (var streamReader = new StreamReader(filepath))
        {
            string line;
            await streamReader.ReadLineAsync();
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                parsedLines.Add(formatParser.ParseLine(line).ToArray());
                if (parsedLines.Count == 1000)
                {
                    yield return parsedLines;
                    parsedLines.Clear();
                }
            }

            if (parsedLines.Count > 0)
                yield return parsedLines;
        }
    }
}