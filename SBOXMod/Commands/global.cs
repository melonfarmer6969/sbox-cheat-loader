using POOP.Components;
using Sandbox.Engine;
using Sandbox.Internal;
using Sandbox.Services.Players;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using POOP.Utils;
using Sandbox;
using Sandbox.Utility;
using Sandbox.Services;

namespace POOP.Commands
{
    public static class Commands
    {

        [ConCmd("log-auth-token")]
        public static async Task LogHWID(string game)
        {
            if (string.IsNullOrEmpty(game))
            {
                Log.Info("game argument is empty");
                return;
            }
            string token = await Auth.GetToken(game);
            Log.Info(token);

        }

        [ConCmd("log-hwid")]
        public static void LogHWID()
        {
            Log.Info("Attempted to spoof HWID");
            Log.Info($"ProcessorName: {SystemInfo.ProcessorName}");
            Log.Info($"ProcessorCount: {SystemInfo.ProcessorCount}");
            Log.Info($"Gpu: {SystemInfo.Gpu}");
            Log.Info($"GpuMemory: {SystemInfo.GpuMemory}");
            Log.Info($"TotalMemory: {SystemInfo.TotalMemory} GB");
        }


            // [ConCmd("add_menu")]
            // public static void AddMenu()
            // {
            //     var go = Game.ActiveScene.CreateObject();
            //     var component = go.AddComponent<ScreenPanel>(true);
            //     component.TargetCamera = Game.ActiveScene.Camera;
            //     component.ZIndex = 1000;
            //     var component2 = go.AddComponent<CheatMenu>(true);

            //     string filePath = @"C:\Inject\css\CheatMenu.razor.scss";
            //     string cssContent = File.ReadAllText(filePath);

            //     if (cssContent != null)
            //     {
            //          component2.Panel.StyleSheet.Add(Sandbox.UI.StyleSheet.FromString(cssContent));

            //     }



            //     if (component != null)
            //     {
            //         Log.Info($"Component created: {component.GetType().Name}");
            //         Log.Info($"OnComponentUpdate null: {component.OnComponentUpdate == null}");
            //         Log.Info($"OnComponentFixedUpdate null: {component.OnComponentFixedUpdate == null}");

            //         // Check if it implements the interfaces
            //         Log.Info($"Is IUpdateSubscriber: {component is IUpdateSubscriber}");
            //         Log.Info($"Is IFixedUpdateSubscriber: {component is IFixedUpdateSubscriber}");
            //         Log.Info($"Is IPreRenderSubscriber: {component is IPreRenderSubscriber}");
            //     }
            // }

            // [ConCmd("check_component_registration")]
            // public static void CheckComponentRegistration()
            // {
            //     var go = Game.ActiveScene.CreateObject();
            //     var component = go.AddComponent<CustomOverlay>(true);

            //     if (component != null)
            //     {
            //         Log.Info($"Component created: {component.GetType().Name}");
            //         Log.Info($"OnComponentUpdate null: {component.OnComponentUpdate == null}");
            //         Log.Info($"OnComponentFixedUpdate null: {component.OnComponentFixedUpdate == null}");

            //         // Check if it implements the interfaces
            //         Log.Info($"Is IUpdateSubscriber: {component is IUpdateSubscriber}");
            //         Log.Info($"Is IFixedUpdateSubscriber: {component is IFixedUpdateSubscriber}");
            //         Log.Info($"Is IPreRenderSubscriber: {component is IPreRenderSubscriber}");
            //     }
            // }

            // [ConCmd("add_overlay")]
            // public static void AddOverlay()
            // {
            //     try
            //     {
            //         Log.Info("=== Starting AddOverlay Debug ===");

            //         // Check if Game is available

            //         // Check ActiveScene
            //         Log.Info($"ActiveScene available: {Game.ActiveScene != null}");
            //         if (Game.ActiveScene == null)
            //         {
            //             Log.Error("Game.ActiveScene is null - cannot create objects");
            //             return;
            //         }

            //         Log.Info($"ActiveScene name: {Game.ActiveScene.Name ?? "unnamed"}");

            //         // Check if we can create objects
            //         Log.Info("Attempting to create GameObject...");
            //         var go = Game.ActiveScene.CreateObject();
            //         Log.Info($"GameObject created: {go != null}");

            //         if (go == null)
            //         {
            //             Log.Error("CreateObject returned null");
            //             return;
            //         }

            //         Log.Info($"GameObject type: {go.GetType().FullName}");
            //         Log.Info($"GameObject valid: {go.IsValid}");
            //         Log.Info($"GameObject enabled: {go.Enabled}");
            //         Log.Info($"GameObject name: {go.Name ?? "unnamed"}");

            //         // Check CustomOverlay type
            //         Log.Info($"CustomOverlay type: {typeof(CustomOverlay).FullName}");
            //         Log.Info($"Is CustomOverlay a Component: {typeof(Component).IsAssignableFrom(typeof(CustomOverlay))}");

            //         // Check if CustomOverlay has parameterless constructor
            //         var constructor = typeof(CustomOverlay).GetConstructor(Type.EmptyTypes);
            //         Log.Info($"CustomOverlay has parameterless constructor: {constructor != null}");

            //         // Try to create instance manually first
            //         Log.Info("Attempting to create CustomOverlay instance manually...");
            //         var manualInstance = new CustomOverlay();
            //         Log.Info($"Manual instance created: {manualInstance != null}");
            //         Log.Info($"Manual instance type: {manualInstance?.GetType().FullName}");

            //         // Check Components system
            //         Log.Info($"GameObject.Components available: {go.Components != null}");

            //         // Try adding the component
            //         Log.Info("Attempting to add component...");
            //         var component = go.AddComponent<CustomOverlay>(true);
            //         Log.Info($"AddComponent returned: {component != null}");

            //         if (component != null)
            //         {
            //             Log.Info($"Component type: {component.GetType().FullName}");
            //             Log.Info($"Component enabled: {component.Enabled}");
            //             Log.Info($"Component valid: {component.IsValid}");
            //         }

            //         // Verify component was added
            //         var foundComponent = go.GetComponent<CustomOverlay>();
            //         Log.Info($"Component found after adding: {foundComponent != null}");

            //         // List all components
            //         var allComponents = go.Components.GetAll();
            //         Log.Info($"Total components on GameObject: {allComponents?.Count() ?? 0}");
            //         if (allComponents != null && allComponents.Any())
            //         {
            //             Log.Info($"Component types: {string.Join(", ", allComponents.Select(c => c.GetType().Name))}");
            //         }

            //         Log.Info("=== AddOverlay Debug Complete ===");
            //     }
            //     catch (Exception ex)
            //     {
            //         Log.Error($"Exception type: {ex.GetType().Name}");
            //         Log.Error($"Failed to add component: {ex.Message}");
            //         Log.Error($"Stack trace: {ex.StackTrace}");

            //         if (ex.InnerException != null)
            //         {
            //             Log.Error($"Inner exception: {ex.InnerException.Message}");
            //             Log.Error($"Inner exception stack trace: {ex.InnerException.StackTrace}");
            //         }
            //     }
            // }





            [ConCmd("log-models")]
        public static void LogModels()
        {
            var models = ResourceLibrary.GetAll<Model>();
            foreach (var model in models)
            {
                Log.Info($"Model: {model.Name} at {model.ResourcePath}");
            }
        }

        [ConCmd("change-model")]
        public static void PlayerToWatermelon(string model = "")
        {

            var name = Connection.Local.Name;



            // try find local player
            var players = Game.ActiveScene.GetAllObjects(true)
                .Where(obj => obj.Name.Contains(Steam.PersonaName)).First();

            var smd = players?.GetComponent<SkinnedModelRenderer>();
            smd.Model = Model.Load( model );

        }



        [ConCmd("change_player_size")]
        public static void ChangePlayerSizeCommand(float size = 1.0f)
        {
            Log.Info($"username {Steam.PersonaName}");

            // Validate size argument
            if (size <= 0)
            {
                Log.Warning("Size must be greater than 0. Using default size of 1.0");
                size = 1.0f;
            }

            var name = Connection.Local.Name;



            // try find local player
            var players = Game.ActiveScene.GetAllObjects(true)
                .Where(obj => obj.Name.Contains(Steam.PersonaName));

            



            foreach (var playerObject in players)
            {
                // Store original scale for reference
                var originalScale = playerObject.WorldScale;

                // Set new scale (uniform scaling)
                playerObject.WorldScale = Vector3.One * size;

                Log.Info($"Changed '{playerObject.Name}' scale from {originalScale} to {playerObject.WorldScale}");
            }

            Log.Info("Player size change completed!");
        }

        private static void LogGameObjectTree(GameObject gameObject, int depth)
        {
            // Create indentation based on depth
            string indent = new string(' ', depth * 2);
            string treePrefix = depth > 0 ? "├─ " : "";

            // Log the main object info
            Log.Info($"{indent}{treePrefix}{gameObject.Name}");
            Log.Info($"{indent}   ID: {gameObject.Id} | Enabled: {gameObject.Enabled}");
            Log.Info($"{indent}   Pos: {gameObject.WorldPosition} | Rot: {gameObject.WorldRotation}");
            Log.Info($"{indent}   Scale: {gameObject.WorldScale}");

            // Log tags if any
      

            // Log components
            var components = gameObject.Components.GetAll();
            if (components.Any())
            {
                Log.Info($"{indent}   Components: {string.Join(", ", components.Select(c => c.GetType().Name))}");
            }

            // Recursively log children
            if (gameObject.Children.Count > 0)
            {
                Log.Info($"{indent}   Children ({gameObject.Children.Count}):");
                foreach (var child in gameObject.Children)
                {
                    LogGameObjectTree(child, depth + 1);
                }
            }

            // Add spacing between root objects
            if (depth == 0)
            {
                Log.Info("");
            }
        }

        // Alternative version with more visual tree structure
        [ConCmd("list_gameobjects_tree_fancy")]
        public static void ListGameObjectsTreeFancyCommand()
        {
            var rootObjects = Game.ActiveScene.GetAllObjects(true).Where(obj => obj.Parent == null);

            Log.Info("=== Scene GameObject Tree (Fancy) ===");
            Log.Info($"Total GameObjects: {Game.ActiveScene.GetAllObjects(true).Count()}");
            Log.Info("");

            foreach (var rootObject in rootObjects)
            {
                LogGameObjectTreeFancy(rootObject, "", true);
            }
        }

        private static void LogGameObjectTreeFancy(GameObject gameObject, string prefix, bool isLast)
        {
            // Create tree-like visual structure
            string connector = isLast ? "└── " : "├── ";
            string childPrefix = isLast ? "    " : "│   ";

            // Main object line
            Log.Info($"{prefix}{connector}{gameObject.Name} [{gameObject.Id}]");

            // Object details with proper indentation
            string detailPrefix = prefix + (isLast ? "    " : "│   ");
            Log.Info($"{detailPrefix}▪ Enabled: {gameObject.Enabled}");
            Log.Info($"{detailPrefix}▪ Position: {gameObject.WorldPosition}");
            Log.Info($"{detailPrefix}▪ Rotation: {gameObject.WorldRotation}");
            Log.Info($"{detailPrefix}▪ Scale: {gameObject.WorldScale}");

       

            var components = gameObject.Components.GetAll();
            if (components.Any())
            {
                Log.Info($"{detailPrefix}▪ Components: {string.Join(", ", components.Select(c => c.GetType().Name))}");
            }

            // Process children
            var children = gameObject.Children.ToList();
            for (int i = 0; i < children.Count; i++)
            {
                bool isLastChild = i == children.Count - 1;
                LogGameObjectTreeFancy(children[i], prefix + childPrefix, isLastChild);
            }
        }
    }
}
