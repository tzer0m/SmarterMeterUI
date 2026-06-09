namespace SmarterMeterUI.Models;

/// <summary>
/// Represents a single meter reading returned from the API.
/// </summary>
public class MeterReading
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public decimal Value { get; set; }
    
    /// <summary>
    /// Raw text
    /// </summary>
    public string? RawText { get; set; }
    
    /// <summary>
    /// Condfidence
    /// </summary>
    public float Confidence { get; set; }

    /// <summary>
    /// Captured at
    /// </summary>
    public DateTime CapturedAt { get; set; }

    /// <summary>
    /// Recorded at
    /// </summary>
    public DateTime RecordedAt { get; set; }
}