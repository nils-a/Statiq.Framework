﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Statiq.Common
{
    /// <summary>
    /// Wraps a file within a virtual directory so that crawling back up to the directory
    /// returns the original virtual directory.
    /// </summary>
    internal class VirtualInputFile : IFile
    {
        private readonly IFile _file;
        private readonly VirtualInputDirectory _directory;

        public VirtualInputFile(IFile file, VirtualInputDirectory directory)
        {
            _file = file.ThrowIfNull(nameof(file));
            _directory = directory.ThrowIfNull(nameof(directory));
        }

        /// <inheritdoc/>
        public NormalizedPath Path => _file.Path;

        /// <inheritdoc/>
        public IDirectory Directory => _directory;

        /// <inheritdoc/>
        public long Length => _file.Length;

        /// <inheritdoc/>
        public string MediaType => _file.MediaType;

        /// <inheritdoc/>
        public bool Exists => _file.Exists;

        /// <inheritdoc/>
        public DateTime LastWriteTime => _file.LastWriteTime;

        /// <inheritdoc/>
        public DateTime CreationTime => _file.CreationTime;

        /// <inheritdoc/>
        public Task CopyToAsync(
            IFile destination,
            bool overwrite = true,
            bool createDirectory = true,
            CancellationToken cancellationToken = default) =>
            _file.CopyToAsync(destination, overwrite, createDirectory, cancellationToken);

        /// <inheritdoc/>
        public void Delete() => _file.Delete();

        /// <inheritdoc/>
        public IContentProvider GetContentProvider() => _file.GetContentProvider();

        /// <inheritdoc/>
        public IContentProvider GetContentProvider(string mediaType) =>
            _file.GetContentProvider(mediaType);

        /// <inheritdoc/>
        public Task MoveToAsync(IFile destination, CancellationToken cancellationToken = default) =>
            _file.MoveToAsync(destination, cancellationToken);

        /// <inheritdoc/>
        public Stream Open(bool createDirectory = true) => _file.Open(createDirectory);

        /// <inheritdoc/>
        public Stream OpenAppend(bool createDirectory = true) => _file.OpenAppend(createDirectory);

        /// <inheritdoc/>
        public Stream OpenRead() => _file.OpenRead();

        /// <inheritdoc/>
        public TextReader OpenText() => _file.OpenText();

        /// <inheritdoc/>
        public Stream OpenWrite(bool createDirectory = true) => _file.OpenWrite(createDirectory);

        /// <inheritdoc/>
        public Task<string> ReadAllTextAsync(CancellationToken cancellationToken = default) =>
            _file.ReadAllTextAsync(cancellationToken);

        /// <inheritdoc/>
        public string ToDisplayString() => _file.ToDisplayString();

        /// <inheritdoc/>
        public Task WriteAllTextAsync(
            string contents,
            bool createDirectory = true,
            CancellationToken cancellationToken = default) =>
            _file.WriteAllTextAsync(contents, createDirectory, cancellationToken);

        /// <inheritdoc/>
        public async Task<int> GetCacheHashCodeAsync() => await _file.GetCacheHashCodeAsync();
    }
}
