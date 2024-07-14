using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Sh.Editor.CodeGenerator
{
    public partial class CodeGenerator : MonoBehaviour
    {
        private static readonly Encoding encoding = new UTF8Encoding(false);

        [MenuItem("ResourceTool/Generate Prefab", false, 1001)]
        public static void GeneratePrefab()
        {
            GenerateConstScript("Prefabs", "Prefab", "prefab");
        }

        [MenuItem("ResourceTool/Generate Scene", false, 1002)]
        public static void GenerateScene()
        {
            GenerateSceneScript();
        }

        private static void GenerateConstScript(string folder, string className, string extension)
        {
            var sb = new StringBuilder();
            var baseDir = $"{Const.ADDRESSABLE_BUNDLE_DIR}/{folder}";
            var filePaths = Directory.GetFiles(baseDir, $"*.{extension}", SearchOption.AllDirectories);
            var directoryPaths = Directory.GetDirectories(baseDir, "*", SearchOption.AllDirectories);
            var groupDirPaths = new HashSet<string>();

            sb.AppendLine($"namespace Sh.Common");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic static partial class Const");
            sb.AppendLine($"\t{{");
            sb.AppendLine($"\t\tpublic static class {className}");
            sb.AppendLine($"\t\t{{");

            foreach (var path in filePaths)
            {
                var resourcePath = path.Replace("\\", "/").Replace($"{baseDir}/", "");
                var constName = resourcePath.Replace($".{extension}", "").Replace("/", "_").ToUpper();

                sb.AppendLine($"\t\t\tpublic const string {constName} = \"{resourcePath}\";");
            }

            foreach (var path in directoryPaths)
            {
                var dirPath = path.Replace("\\", "/").Replace($"{baseDir}/", "");

                groupDirPaths.Add(dirPath);
            }

            if (groupDirPaths.Count > 0)
                groupDirPaths.Distinct();

            sb.AppendLine($"\t\t}}");
            sb.AppendLine();
            sb.AppendLine($"\t\tpublic static class {className}Dir");
            sb.AppendLine($"\t\t{{");
            foreach (var groupPath in groupDirPaths)
            {
                var constName = groupPath.Replace("/", "_").ToUpper();
                sb.AppendLine($"\t\t\tpublic const string {constName} = \"{groupPath}\";");
            }
            sb.AppendLine($"\t\t}}");
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            using (var writer = new StreamWriter($"{Const.CONST_DIR}/Const_{className}.cs", false, encoding))
            {
                writer.Write(sb.ToString());
            }
        }

        private static void GenerateSceneScript()
        {
            var folder = "Scenes";
            var className = "Scene";
            var extension = "unity";
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine($"namespace Sh.Common");
            sb.AppendLine($"{{");
            sb.AppendLine($"\tpublic static partial class Const");
            sb.AppendLine($"\t{{");
            sb.AppendLine($"\t\tpublic static class {className}");
            sb.AppendLine($"\t\t{{");

            var baseDir = $"{Const.ADDRESSABLE_BUNDLE_DIR}/{folder}";
            var filePaths = Directory.GetFiles(baseDir, $"*.{extension}", SearchOption.AllDirectories);

            var groupDirPaths = new HashSet<string>();

            sb.AppendLine($"\t\t\t// BuiltIn");
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    var fileName = Path.GetFileNameWithoutExtension(scene.path);
                    sb.AppendLine($"\t\t\tpublic const string {fileName.ToUpper()} = \"{fileName}\";");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"\t\t\t// Addressable");
            foreach (var path in filePaths)
            {
                var resourcePath = path.Replace("\\", "/").Replace($"{baseDir}/", "");
                var constName = resourcePath.Replace($".{extension}", "").Replace("/", "_").ToUpper();

                sb.AppendLine($"\t\t\tpublic const string {constName} = \"{Path.GetFileNameWithoutExtension(resourcePath)}\";");

                var slashCount = resourcePath.Count(f => f == '/');
                if (slashCount > 0)
                {
                    var fileName = resourcePath.Split('/').Last();
                    var groupPath = resourcePath.Replace($"/{fileName}", "");
                    groupDirPaths.Add(groupPath);
                }
            }

            sb.AppendLine($"\t\t}}");
            sb.AppendLine($"\t}}");
            sb.AppendLine($"}}");

            using (var writer = new StreamWriter($"{Const.CONST_DIR}/Const_{className}.cs", false, encoding))
            {
                writer.Write(sb.ToString());
            }
        }
    }
}
