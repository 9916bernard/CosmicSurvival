using System;
using System.IO;
using System.Collections.Generic;


namespace Table.Exporter
{
    partial class Maker
    {
        public static void WriteManager()
        {
            // Code Client
            //CurrentState.SheetName = "== Write TBManager Header (Client) ==";

            //if (Settings.Code_Client.Count > 0)
            //{
            //    WriteManager_Client(Settings.Code_Client[0]);
            //}

            //for (int i = 1; i < Settings.Code_Client.Count; ++i)
            //{
            //    Util.MakeFolder(Settings.Code_Client[i] + Settings.DATA_DIR);

            //    File.Copy(string.Format("{0}{1}TABLE.h", Settings.Code_Client[0], Settings.DATA_DIR),
            //              string.Format("{0}{1}TABLE.h", Settings.Code_Client[i], Settings.DATA_DIR), true);
            //}


            // Code Client For Unity
            CurrentState.SheetName = "== Write TBManager Header (Client) ==";

            if (Settings.Code_Client.Count > 0)
            {
                WriteManager_Client_Unity(Settings.Code_Client[0]);
            }

            for (int i = 1; i < Settings.Code_Client.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Client[i] + Settings.DATA_DIR);

                File.Copy(string.Format("{0}{1}TABLE.cs", Settings.Code_Client[0], Settings.DATA_DIR),
                          string.Format("{0}{1}TABLE.cs", Settings.Code_Client[i], Settings.DATA_DIR), true);
            }


            // Code Server
            CurrentState.SheetName = "== Write TBManager Header (Server) ==";

            if (Settings.Code_Server.Count > 0)
            {
                WriteManager_Server(Settings.Code_Server[0]);
            }

            for (int i = 1; i < Settings.Code_Server.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Server[i] + Settings.DATA_DIR);

                File.Copy(string.Format("{0}{1}TABLE.h", Settings.Code_Server[0], Settings.DATA_DIR),
                          string.Format("{0}{1}TABLE.h", Settings.Code_Server[i], Settings.DATA_DIR), true);
            }


            // Code Bot
            CurrentState.SheetName = "== Write TBManager Header (Bot) ==";

            if (Settings.Code_Bot.Count > 0)
            {
                // 서버와 같은 파일을 씀
                WriteManager_Server(Settings.Code_Bot[0]);
            }

            for (int i = 1; i < Settings.Code_Bot.Count; ++i)
            {
                Util.MakeFolder(Settings.Code_Bot[i] + Settings.DATA_DIR);

                File.Copy(string.Format("{0}{1}TABLE.h", Settings.Code_Bot[0], Settings.DATA_DIR),
                          string.Format("{0}{1}TABLE.h", Settings.Code_Bot[i], Settings.DATA_DIR), true);
            }
        }

        static public void WriteManager_Client(string outputPath)
        {
            string dir = outputPath + Settings.DATA_DIR;

            Util.MakeFolder(dir);


            string filepath = string.Format("{0}TABLE.h", dir);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("#pragma once");
            w.WriteLine("");

            //w.WriteLine("#include \"TBString_System.h\"");
            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("#include \"TB{0}.h\"", TBL_Config.DefineList[i].mTableName);
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("#include \"TB{0}.h\"", TBL_Normal.DefineList[i].mTableName);
            }

            w.WriteLine("");
            w.WriteLine("#include \"TABLE.generated.h\"");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("UCLASS(Blueprintable, BlueprintType)");
            w.WriteLine("class {0} UTABLE : public UObject", Settings.API);
            w.WriteLine("{");
            w.WriteLine("GENERATED_BODY()");
            w.WriteLine("");
            w.WriteLine("public:");

            //w.WriteLine("{0}UPROPERTY(EditAnywhere) UTBString_System* string_system;", Util.Tab);
            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}UPROPERTY(EditAnywhere) UTB{1}* {2};", Util.Tab, TBL_Config.DefineList[i].mTableName, TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}UPROPERTY(EditAnywhere) UTB{1}* {2};", Util.Tab, TBL_Normal.DefineList[i].mTableName, TBL_Normal.DefineList[i].mTableName.ToLower());
            }

            w.WriteLine("");

            w.WriteLine("{0}void LoadAll()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);

            //w.WriteLine("{0}string_system = NewObject<UTBString_System>(this); string_system->Load();", Util.Tab + Util.Tab);
            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}{1} = NewObject<UTB{2}>(this); {3}->Load();", Util.Tab + Util.Tab,
                    TBL_Config.DefineList[i].mTableName.ToLower(), TBL_Config.DefineList[i].mTableName, TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}{1} = NewObject<UTB{2}>(this); {3}->Load();", Util.Tab + Util.Tab,
                    TBL_Normal.DefineList[i].mTableName.ToLower(), TBL_Normal.DefineList[i].mTableName, TBL_Normal.DefineList[i].mTableName.ToLower());
            }

            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("};");

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteManager_Client_Unity(string outputPath)
        {
            string dir = outputPath + Settings.DATA_DIR;

            Util.MakeFolder(dir);


            string filepath = string.Format("{0}TABLE.cs", dir);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("using UnityEngine;");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("public class TABLE : Singleton<TABLE>");
            w.WriteLine("{");

            //w.WriteLine("{0}private static TABLE msInstance = null;", Util.Tab);
            //w.WriteLine("");
            //w.WriteLine("{0}static public TABLE Inst()", Util.Tab);
            //w.WriteLine("{0}{{", Util.Tab);
            //w.WriteLine("{0}return msInstance;", Util.Tab + Util.Tab);
            //w.WriteLine("{0}}}", Util.Tab);
            //w.WriteLine("");
            //w.WriteLine("{0}private void Awake()", Util.Tab);
            //w.WriteLine("{0}{{", Util.Tab);
            //w.WriteLine("{0}if (msInstance == null)", Util.Tab + Util.Tab);
            //w.WriteLine("{0}{{", Util.Tab + Util.Tab);
            //w.WriteLine("{0}msInstance = this;", Util.Tab + Util.Tab + Util.Tab);
            //w.WriteLine("{0}}}", Util.Tab + Util.Tab);
            //w.WriteLine("{0}}}", Util.Tab);
            //w.WriteLine("");

            //w.WriteLine("{0}private bool mInit = false;", Util.Tab);
            //w.WriteLine("");

            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}static public UTB{1} {2} = null;", Util.Tab, TBL_Config.DefineList[i].mTableName, TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}static public UTB{1} {2} = null;", Util.Tab, TBL_Normal.DefineList[i].mTableName, TBL_Normal.DefineList[i].mTableName.ToLower());
            }
            w.WriteLine("");

            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}public UTB{1} o_{2} = new UTB{3}();", Util.Tab, TBL_Config.DefineList[i].mTableName, TBL_Config.DefineList[i].mTableName.ToLower(), TBL_Config.DefineList[i].mTableName);
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}public UTB{1} o_{2} = new UTB{3}();", Util.Tab, TBL_Normal.DefineList[i].mTableName, TBL_Normal.DefineList[i].mTableName.ToLower(), TBL_Normal.DefineList[i].mTableName);
            }
            w.WriteLine("");

            w.WriteLine("{0}public void LoadAll(bool InForceResources)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);

            //w.WriteLine("{0}if (mInit == true) {{ return; }}", Util.Tab + Util.Tab);
            //w.WriteLine("");
            //w.WriteLine("{0}mInit = true;", Util.Tab + Util.Tab);
            //w.WriteLine("");

            w.WriteLine("{0}byte[] buffer = new byte[4096];", Util.Tab + Util.Tab);
            w.WriteLine("");

            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}o_{1}.Load(buffer, InForceResources);", Util.Tab + Util.Tab, TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}o_{1}.Load(buffer, InForceResources);", Util.Tab + Util.Tab, TBL_Normal.DefineList[i].mTableName.ToLower());
            }
            w.WriteLine("");
            w.WriteLine("{0}RestoreStaticModule();", Util.Tab + Util.Tab);

            w.WriteLine("{0}}}", Util.Tab);
            w.WriteLine("");


            w.WriteLine("{0}public override void RestoreStaticModule()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}base.RestoreStaticModule();", Util.Tab + Util.Tab);
            w.WriteLine("");
            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}{1} = o_{1};", Util.Tab + Util.Tab, TBL_Config.DefineList[i].mTableName.ToLower(), TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}{1} = o_{1};", Util.Tab + Util.Tab, TBL_Normal.DefineList[i].mTableName.ToLower(), TBL_Normal.DefineList[i].mTableName.ToLower());
            }
            w.WriteLine("{0}}}", Util.Tab);
            w.WriteLine("");


            w.WriteLine("{0}public int GetLastVersion(string InTableName)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}string tableName = InTableName.ToLower();", Util.Tab + Util.Tab);
            w.WriteLine("");
            w.WriteLine("{0}int storageType = 0;", Util.Tab + Util.Tab);
            w.WriteLine("");
            w.WriteLine("{0}switch (tableName)", Util.Tab + Util.Tab);
            w.WriteLine("{0}{{", Util.Tab + Util.Tab);
            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}case \"{1}\": return {2}.GetLastVersion(out storageType);", Util.Tab + Util.Tab + Util.Tab,
                    TBL_Config.DefineList[i].mTableName.ToLower(), TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}case \"{1}\": return {2}.GetLastVersion(out storageType);", Util.Tab + Util.Tab + Util.Tab,
                    TBL_Normal.DefineList[i].mTableName.ToLower(), TBL_Normal.DefineList[i].mTableName.ToLower());
                //w.WriteLine("{0}{1} = o_{1};", Util.Tab + Util.Tab, TBL_Normal.DefineList[i].mTableName.ToLower(), TBL_Normal.DefineList[i].mTableName.ToLower());
            }
            w.WriteLine("{0}}}", Util.Tab + Util.Tab);
            w.WriteLine("");
            w.WriteLine("{0}return 0;", Util.Tab + Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);


            w.WriteLine("};");

            Util.FlushAndReleaseTextFile(w);
        }

        static public void WriteManager_Server(string outputPath)
        {
            string dir = outputPath + Settings.DATA_DIR;

            Util.MakeFolder(dir);


            string filepath = string.Format("{0}TABLE.h", dir);
            StreamWriter w = Util.GetNewTextFile(filepath);


            w.WriteLine("#pragma once");
            w.WriteLine("");

            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("#include \"TB{0}.h\"", TBL_Config.DefineList[i].mTableName);
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("#include \"TB{0}.h\"", TBL_Normal.DefineList[i].mTableName);
            }

            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("class UTABLE");
            w.WriteLine("{");
            w.WriteLine("public:");
            w.WriteLine("{0}int32 loading_version = 0;", Util.Tab);
            w.WriteLine("{0}int32 use_count = 0;", Util.Tab);
            w.WriteLine("");
            w.WriteLine("{0}class TableExtension* extension = nullptr;", Util.Tab);
            w.WriteLine("");

            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}UTB{1}* {2};", Util.Tab, TBL_Config.DefineList[i].mTableName, TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}UTB{1}* {2};", Util.Tab, TBL_Normal.DefineList[i].mTableName, TBL_Normal.DefineList[i].mTableName.ToLower());
            }

            w.WriteLine("");

            w.WriteLine("{0}void LoadAll()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);

            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}{1} = new UTB{2}(); {3}->Load();", Util.Tab + Util.Tab,
                    TBL_Config.DefineList[i].mTableName.ToLower(), TBL_Config.DefineList[i].mTableName, TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}{1} = new UTB{2}(); {3}->Load();", Util.Tab + Util.Tab,
                    TBL_Normal.DefineList[i].mTableName.ToLower(), TBL_Normal.DefineList[i].mTableName, TBL_Normal.DefineList[i].mTableName.ToLower());
            }

            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}void UnloadAll()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);

            //TBL_Config
            for (int i = 0; i < TBL_Config.DefineList.Count; ++i)
            {
                w.WriteLine("{0}delete {1};", Util.Tab + Util.Tab, TBL_Config.DefineList[i].mTableName.ToLower());
            }
            //TBL_String
            //TBL_Normal
            for (int i = 0; i < TBL_Normal.DefineList.Count; ++i)
            {
                w.WriteLine("{0}{1}->Unload(); delete {2};", Util.Tab + Util.Tab,
                    TBL_Normal.DefineList[i].mTableName.ToLower(), TBL_Normal.DefineList[i].mTableName.ToLower());
            }

            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("};");

            Util.FlushAndReleaseTextFile(w);
        }
    }
}
