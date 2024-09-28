﻿using Open.Core.SeedWork.Interfaces;
using Open.Driver.Domain.SeedWork;

namespace Open.Driver.Domain.Aggregates;

public class File : EntityBase, IAggregateRoot
{
    public string FileName { get; set; }

    public string OriginalFileName { get; set; }

    public string FileExtension { get; set; }
    
    public ContentType ContentType { get; set; }
    
    public string Notes { get; set; } // Ghi chú thêm
    
    public string Description { get; set; }

    public string Tags { get; set; }

    public long Size { get; set; }

    public Guid DirectoryId { get; set; }
    
    public virtual Directory Directory { get; set; }
}