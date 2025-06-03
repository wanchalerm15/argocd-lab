using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default));

var sampleTodos = new Todo[] {
    new(1, "สมุดจด"),
    new(2, "ดินน้ำมัน", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "ปากกา", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "ดินสอ"),
    new(5, "ไม่บรรทัด", DateOnly.FromDateTime(DateTime.Now.AddDays(2))),
    new(5, "ยางลบ", DateOnly.FromDateTime(DateTime.Now.AddDays(2))),
};

var app = builder.Build();
app.MapGet("/", () => sampleTodos);
app.MapGet("/{id}", (int id) => sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo ? Results.Ok(todo) : Results.NotFound());
app.Run();

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false); 
