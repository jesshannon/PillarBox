${
    // Enable extension methods by adding using Typewriter.Extensions.*
    using Typewriter.Extensions.Types;

    // Uncomment the constructor to change template settings.
    //Template(Settings settings)
    //{
    //    settings.IncludeProject("Project.Name");
    //    settings.OutputExtension = ".tsx";
    //}

    // Custom extension methods can be used in the template by adding a $ prefix e.g. $LoudName
    string LoudName(Property property)
    {
        return property.Name.ToUpperInvariant();
    }

	Template(Settings settings)
    {

        settings.OutputFilenameFactory = (file) => {
			return "../ClientApp/src/app/models/" + file.Name.Replace(".cs",".model.ts");
		};
        
        settings.OutputExtension = ".ts";
    }

    
    // Custom extension methods can be used in the template by adding a $ prefix
    Type[] CalculatedModelTypes(Class c)
    {
        return c.Properties.Select(p=>p.Type)
            .SelectMany(t => 
                t.TypeArguments.Where(a=>a.Namespace.StartsWith("PillarBox.Web.Models"))
                    .Append(t.Namespace.StartsWith("PillarBox.Web.Model") ? t : null)
            )
            .Where(t=> 
                t != null &&
                t.FullName!=c.FullName &&
                !c.TypeArguments.Any(ct=>ct.FullName == t.FullName)
                )
            .ToArray();

    }

    string ImportName(Type t)
    {
        return t.OriginalName;
    }

}
// More info: http://frhagn.github.io/Typewriter/
$Classes(PillarBox.Web.Models.*)[

$CalculatedModelTypes[
import { $ImportName } from './$ImportName.model';]

export class $Name$TypeParameters {
    $Properties[
    // $LoudName
    public $name: $Type = $Type[$Default];]
}]
