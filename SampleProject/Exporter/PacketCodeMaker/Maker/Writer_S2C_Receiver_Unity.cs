using System;
using System.IO;
using System.Collections.Generic;


namespace PacketCodeMaker
{
    public class Writer_S2C_Receiver_Unity
    {
        static public void Write(Util.PathInfo pathInfo, PacketInfos infos)
        {
            string filepath_client = string.Format("{0}NetReceiver_{1}.cs", pathInfo.mCodePathClient, infos.mServerName);

            Write_Code(filepath_client, infos);
            Console.WriteLine("=== Write {0}", filepath_client);
        }

        static private void Write_Code(string filepath_client, PacketInfos infos)
        {
            StreamWriter w = Util.GetTextFile(filepath_client);


            w.WriteLine("using System;");
            w.WriteLine("using System.Collections.Generic;");
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("namespace TcpNet");
            w.WriteLine("{");
            w.WriteLine("");

            w.WriteLine("{0}public partial class NetReceiver_{1} : NetReceiver", Util.Tab, infos.mServerName);
            w.WriteLine("{0}{{", Util.Tab);


            Write_Code_Constructor(w, infos, Util.Tab + Util.Tab);

            w.WriteLine("");


            Write_Code_AddFunc(w, infos, Util.Tab + Util.Tab);

            w.WriteLine("");


            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");
            w.WriteLine("} // end of namespace TcpNet");



            // Proc 코드 주석으로 생성
            //w.WriteLine(""); w.WriteLine(""); w.WriteLine("/*");
            //w.WriteLine("private void OnConnect(bool success)"); w.WriteLine("{"); w.WriteLine("}"); w.WriteLine("");
            //w.WriteLine("private void OnDisconnect()"); w.WriteLine("{"); w.WriteLine("}"); w.WriteLine("");
            //w.WriteLine("private void OnNotExist(int cmd)"); w.WriteLine("{"); w.WriteLine("}"); w.WriteLine("");
            //foreach (PacketInfos.Packet p in infos.mPackets.Values)
            //{
            //    if (p.way == "S2C")
            //    {
            //        w.WriteLine("private void {0}(byte[] data, int start)", p.cmd_name); w.WriteLine("{"); w.WriteLine("}"); w.WriteLine("");
            //    }
            //}
            //w.WriteLine("*/");



            Util.FlushAndReleaseTextFile(w);
        }

        static private void Write_Code_Constructor(StreamWriter w, PacketInfos infos, string tab)
        {
            w.WriteLine("{0}public NetReceiver_{1}()", tab, infos.mServerName);
            w.WriteLine("{0}{{", tab);

            w.WriteLine("{0}RegistOnConnect(OnConnect);", tab + Util.Tab);
            w.WriteLine("{0}RegistOnDisconnect(OnDisconnect);", tab + Util.Tab);
            w.WriteLine("{0}RegistOnNotExist(OnNotExist);", tab + Util.Tab);

            w.WriteLine("");

            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "S2C")
                {
                    w.WriteLine("{0}AddFunc(NP_{1}_S2C.{2}, {3});", tab + Util.Tab, infos.mServerNameUP, p.cmd_name.ToUpper(), p.cmd_name);
                }
            }

            w.WriteLine("{0}}}", tab);
        }

        static private void Write_Code_AddFunc(StreamWriter w, PacketInfos infos, string tab)
        {
            w.WriteLine("{0}private void AddFunc(NP_{1}_S2C cmd, Action<byte[], int> action)", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}RegistFunc((int)cmd, action);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}public void AddFuncPersist(NP_{1}_S2C cmd, Action<byte[], int> action)", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}RegistFuncPersist((int)cmd, action);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}public void RemoveFuncPersist(NP_{1}_S2C cmd)", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}UnRegistFuncPersist((int)cmd);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
            w.WriteLine("");

            w.WriteLine("{0}public void AddFuncOnce(NP_{1}_S2C cmd, Action<byte[], int> action)", tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", tab);
            w.WriteLine("{0}RegistFuncOnce((int)cmd, action);", tab + Util.Tab);
            w.WriteLine("{0}}}", tab);
        }

        static private void Write_Header(string filepath_client, string filepath_bot, PacketInfos infos)
        {
            StreamWriter w = Util.GetTextFile(filepath_client);


            List<PacketInfos.Packet> s2cList = new List<PacketInfos.Packet>();
            foreach (PacketInfos.Packet p in infos.mPackets.Values)
            {
                if (p.way == "S2C")
                {
                    s2cList.Add(p);
                }
            }


            w.WriteLine("#pragma once");
            w.WriteLine("");
            w.WriteLine("#include \"CoreMinimal.h\"");
            w.WriteLine("#include \"./Lib/NetReceiver.h\"");
            w.WriteLine("#include \"./Packet/NetPacket_{0}.h\"", infos.mServerName);
            w.WriteLine("#include \"Net_{0}_S2C.generated.h\"", infos.mServerName);
            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("UCLASS()");
            w.WriteLine("class {0} UNet_{1}_S2C : public UNetReceiver", Util.API, infos.mServerName);
            w.WriteLine("{");
            w.WriteLine("{0}GENERATED_BODY()", Util.Tab);
            w.WriteLine("");

            w.WriteLine("private:");

            w.WriteLine("{0}TMap<NP_{1}_S2C, Dele_Packet> mFuncMap;", Util.Tab, infos.mServerNameUP);
            w.WriteLine("{0}TMap<NP_{1}_S2C, Dele_Packet> mFuncOnceMap;", Util.Tab, infos.mServerNameUP);
            w.WriteLine("{0}TMap<NP_{1}_S2C, TFunction<void(uint8*)>> mLambdaOnceMap;", Util.Tab, infos.mServerNameUP);

            w.WriteLine("");

            w.WriteLine("public:");

            w.WriteLine("{0}virtual void OnReceive(uint8* data)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}{1}NP{2}S2CHeader* header = (NP{3}S2CHeader*)data;", Util.Tab, Util.Tab, infos.mServerName, infos.mServerName);
            w.WriteLine("");
            w.WriteLine("{0}{1}Dele_Packet* func = mFuncMap.Find(header->GetCmd());", Util.Tab, Util.Tab);
            w.WriteLine("");
            w.WriteLine("{0}{1}if (func != nullptr)", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{{", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}(*func).ExecuteIfBound(data);", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}{1}{2}Dele_Packet* funconce = mFuncOnceMap.Find(header->GetCmd());", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}if (funconce != nullptr)", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}{{", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}{3}(*funconce).ExecuteIfBound(data);", Util.Tab, Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}{3}mFuncOnceMap.Remove(header->GetCmd());", Util.Tab, Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}}}", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("");

            w.WriteLine("{0}{1}{2}TFunction<void(uint8*)>* lambda = mLambdaOnceMap.Find(header->GetCmd());", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}if (lambda != nullptr)", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}{{", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}{3}(*lambda)(data);", Util.Tab, Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}{3}mLambdaOnceMap.Remove(header->GetCmd());", Util.Tab, Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}}}", Util.Tab, Util.Tab, Util.Tab);

            w.WriteLine("{0}{1}}}", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}else", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{{", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}mOnNotExist.ExecuteIfBound(data);", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}}}", Util.Tab, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}template<class T>", Util.Tab);
            w.WriteLine("{0}void RegistFunc(NP_{1}_S2C cmd, T* p, void(T::* f)(uint8*))", Util.Tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}{1}mFuncMap.Add(cmd, Dele_Packet::CreateUObject(p, f));", Util.Tab, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}template<class T>", Util.Tab);
            w.WriteLine("{0}void RegistFuncOnce(NP_{1}_S2C cmd, T* p, void(T::* f)(uint8*))", Util.Tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}{1}if (mFuncOnceMap.Contains(cmd) == true)", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{{", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}// 같은 패킷으로 이미 등록된 함수가 있으면 제거한다.", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}// 인스턴스가 제거된 경우 호출 시 크래쉬난다.", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}mFuncOnceMap.Remove(cmd);", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}}}", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}mFuncOnceMap.Add(cmd, Dele_Packet::CreateUObject(p, f));", Util.Tab, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}void RegistLambdaOnce(NP_{1}_S2C cmd, TFunction<void(uint8*)> f)", Util.Tab, infos.mServerNameUP);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}{1}if (mLambdaOnceMap.Contains(cmd) == true)", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{{", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}// 같은 패킷으로 이미 등록된 람다가 있으면 제거한다.", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}// 람다가 캡쳐한 스코프가 제거된 경우 호출 시 크래쉬난다.", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}{2}mLambdaOnceMap.Remove(cmd);", Util.Tab, Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}}}", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}mLambdaOnceMap.Add(cmd, f);", Util.Tab, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}void ClearFuncOnce()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}{1}mFuncOnceMap.Empty();", Util.Tab, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}void ClearLambdaOnce()", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}{1}mLambdaOnceMap.Empty();", Util.Tab, Util.Tab);
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("");

            w.WriteLine("{0}template<class T>", Util.Tab);
            w.WriteLine("{0}void RegisterFunc(T* p)", Util.Tab);
            w.WriteLine("{0}{{", Util.Tab);
            w.WriteLine("{0}{1}mOnConnect = Dele_Connect::CreateUObject(p, &T::OnConnect);", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}mOnDisconnect = Dele_Disconnect::CreateUObject(p, &T::OnDisconnect);", Util.Tab, Util.Tab);
            w.WriteLine("{0}{1}mOnNotExist = Dele_Packet::CreateUObject(p, &T::OnNotExist);", Util.Tab, Util.Tab);
            w.WriteLine("");
            for (int i = 0; i < s2cList.Count; ++i)
            {
                w.WriteLine("{0}{1}RegistFunc(NP_{2}_S2C::{3}, p, &T::On{4});"
                    , Util.Tab, Util.Tab, infos.mServerNameUP, s2cList[i].cmd_name.ToUpper(), s2cList[i].cmd_name);
            }
            w.WriteLine("{0}}}", Util.Tab);

            w.WriteLine("};");

            w.WriteLine("");
            w.WriteLine("");


            //Write_Header_Macro(w, infos);


            //w.WriteLine("");
            //w.WriteLine("");



            //Write_Header_MacroExt(w, infos, s2cList);


            //w.WriteLine("");
            //w.WriteLine("");
            //w.WriteLine("");


            //Write_Header_MacroUsage(w, infos, s2cList);


            Util.FlushAndReleaseTextFile(w);

            File.Copy(filepath_client, filepath_bot, true);
        }

        static private void Write_Header_Macro(StreamWriter w, PacketInfos infos)
        {
            //w.WriteLine("#define DELEGATE_{0}_END }}", infos.mServerNameUP);
            //w.WriteLine("");

            //w.WriteLine("#define DELEGATE_{0}_ONCONNECT_DEFINE void OnConnect(bool success);", infos.mServerNameUP);
            //w.WriteLine("#define DELEGATE_{0}_ONCONNECT_START(CLASS) void CLASS::OnConnect(bool success) {{", infos.mServerNameUP);
            w.WriteLine("#define DELEGATE_{0}_ONCONNECT_REGIST(S2C, CLASS, CLASSPTR) S2C->RegistOnConnect(SA::delegate<void(bool)>::create<CLASS, &CLASS::OnConnect>(CLASSPTR));", infos.mServerNameUP);
            //w.WriteLine("");

            //w.WriteLine("#define DELEGATE_{0}_ONDISCONNECT_DEFINE void OnDisconnect();", infos.mServerNameUP);
            //w.WriteLine("#define DELEGATE_{0}_ONDISCONNECT_START(CLASS) void CLASS::OnDisconnect() {{", infos.mServerNameUP);
            w.WriteLine("#define DELEGATE_{0}_ONDISCONNECT_REGIST(S2C, CLASS, CLASSPTR) S2C->RegistOnDisconnect(SA::delegate<void()>::create<CLASS, &CLASS::OnDisconnect>(CLASSPTR));", infos.mServerNameUP);
            //w.WriteLine("");

            //w.WriteLine("#define DELEGATE_{0}_ONNOTEXIST_DEFINE void OnNotExist(NP{1}S2CHeader* p);", infos.mServerNameUP, infos.mServerName);
            //w.WriteLine("#define DELEGATE_{0}_ONNOTEXIST_START(CLASS) void CLASS::OnNotExist(NP{1}S2CHeader* p) {{", infos.mServerNameUP, infos.mServerName);
            w.WriteLine("#define DELEGATE_{0}_ONNOTEXIST_REGIST(S2C, CLASS, CLASSPTR) S2C->RegistOnNotExist(SA::delegate<void(NP{1}S2CHeader*)>::create<CLASS, &CLASS::OnNotExist>(CLASSPTR));", infos.mServerNameUP, infos.mServerName);
            w.WriteLine("");

            //w.WriteLine("#define DELEGATE_{0}_DEFINE(PACKET) void On##PACKET(uint8* p);", infos.mServerNameUP);
            //w.WriteLine("#define DELEGATE_{0}_START(CLASS, PACKET) void CLASS::On##PACKET(uint8* p) {{ NP{1}S2C_##PACKET& d = *((NP{2}S2C_##PACKET*)p);", infos.mServerNameUP, infos.mServerName, infos.mServerName);
            w.WriteLine("#define DELEGATE_{0}_REGIST(S2C, CLASS, CLASSPTR, CMD, PACKET) S2C->RegistFunc(CMD, SA::delegate<void(uint8*)>::create<CLASS, &CLASS::On##PACKET>(CLASSPTR));", infos.mServerNameUP);
        }

        static private void Write_Header_MacroExt(StreamWriter w, PacketInfos infos, List<PacketInfos.Packet> s2cList)
        {
            //for (int i = 0; i < s2cList.Count; ++i)
            //{
            //    w.WriteLine("#define DELEGATE_{0}_DEFINE_{1} DELEGATE_{2}_DEFINE({3})", infos.mServerNameUP, s2cList[i].cmd_name, infos.mServerNameUP, s2cList[i].cmd_name);
            //}

            //w.WriteLine("");

            //for (int i = 0; i < s2cList.Count; ++i)
            //{
            //    w.WriteLine("#define DELEGATE_{0}_START_{1}(CLASS) DELEGATE_{2}_START(CLASS, {3})", infos.mServerNameUP, s2cList[i].cmd_name, infos.mServerNameUP, s2cList[i].cmd_name);
            //}

            //w.WriteLine("");

            for (int i = 0; i < s2cList.Count; ++i)
            {
                w.WriteLine("#define DELEGATE_{0}_REGIST_{1}(S2C, CLASS, CLASSPTR) DELEGATE_{2}_REGIST(S2C, CLASS, CLASSPTR, NP_{3}_S2C::{4}, {5})", infos.mServerNameUP, s2cList[i].cmd_name, infos.mServerNameUP, infos.mServerNameUP, s2cList[i].cmd_name.ToUpper(), s2cList[i].cmd_name);
            }
        }

        static private void Write_Header_MacroUsage(StreamWriter w, PacketInfos infos, List<PacketInfos.Packet> s2cList)
        {
            w.WriteLine("/*");

            w.WriteLine("//==========================================================================");
            w.WriteLine("// 선언부");
            w.WriteLine("//==========================================================================");
            //w.WriteLine("DELEGATE_LOGIN_ONCONNECT_DEFINE");
            //w.WriteLine("DELEGATE_LOGIN_ONDISCONNECT_DEFINE");
            //w.WriteLine("DELEGATE_LOGIN_ONNOTEXIST_DEFINE");
            w.WriteLine("void OnConnect(bool success);");
            w.WriteLine("void OnDisconnect();");
            w.WriteLine("void OnNotExist(NP{0}S2CHeader* p);", infos.mServerName);
            w.WriteLine("");
            //w.WriteLine("void RegistPacketDelegate();");
            //w.WriteLine("");
            for (int i = 0; i < s2cList.Count; ++i)
            {
                //w.WriteLine("DELEGATE_{0}_DEFINE_{1}", infos.mServerNameUP, s2cList[i].cmd_name);
                w.WriteLine("void On{0}(uint8* p);", s2cList[i].cmd_name);
            }

            w.WriteLine("");
            w.WriteLine("");

            w.WriteLine("//==========================================================================");
            w.WriteLine("// 구현부");
            w.WriteLine("//==========================================================================");
            //w.WriteLine("DELEGATE_{0}_ONCONNECT_START(CLASS)", infos.mServerNameUP);
            //w.WriteLine("DELEGATE_{0}_END", infos.mServerNameUP);
            //w.WriteLine("");
            //w.WriteLine("DELEGATE_{0}_ONDISCONNECT_START(CLASS)", infos.mServerNameUP);
            //w.WriteLine("DELEGATE_{0}_END", infos.mServerNameUP);
            //w.WriteLine("");
            //w.WriteLine("DELEGATE_{0}_ONNOTEXIST_START(CLASS)", infos.mServerNameUP);
            //w.WriteLine("DELEGATE_{0}_END", infos.mServerNameUP);
            //w.WriteLine("");
            //w.WriteLine("");
            w.WriteLine("void CLASS::OnConnect(bool success)");
            w.WriteLine("{");
            w.WriteLine("}");
            w.WriteLine("");
            w.WriteLine("void CLASS::OnDisconnect()");
            w.WriteLine("{");
            w.WriteLine("}");
            w.WriteLine("");
            w.WriteLine("void CLASS::OnNotExist(NP{0}S2CHeader* p)", infos.mServerName);
            w.WriteLine("{");
            w.WriteLine("}");
            w.WriteLine("");
            w.WriteLine("");
            for (int i = 0; i < s2cList.Count; ++i)
            {
                //w.WriteLine("DELEGATE_{0}_START_{1}(CLASS)", infos.mServerNameUP, s2cList[i].cmd_name);
                //w.WriteLine("DELEGATE_{0}_END", infos.mServerNameUP);
                //w.WriteLine("");
                w.WriteLine("void CLASS::On{0}(uint8* p)", s2cList[i].cmd_name);
                w.WriteLine("{");
                w.WriteLine("{0}NP{1}S2C_{2}& d = *((NP{3}S2C_{4}*)p);", Util.Tab, infos.mServerName, s2cList[i].cmd_name, infos.mServerName, s2cList[i].cmd_name);
                w.WriteLine("}");
                w.WriteLine("");
            }

            //w.WriteLine("");


            //w.WriteLine("//==========================================================================");
            //w.WriteLine("// 등록");
            //w.WriteLine("//==========================================================================");
            //w.WriteLine("void CLASS::RegistPacketDelegate()");
            //w.WriteLine("{");
            //w.WriteLine("{0}DELEGATE_{1}_ONCONNECT_REGIST(S2C, CLASS, this)", Util.Tab, infos.mServerNameUP);
            //w.WriteLine("{0}DELEGATE_{1}_ONDISCONNECT_REGIST(S2C, CLASS, this)", Util.Tab, infos.mServerNameUP);
            //w.WriteLine("{0}DELEGATE_{1}_ONNOTEXIST_REGIST(S2C, CLASS, this)", Util.Tab, infos.mServerNameUP);
            //w.WriteLine("");
            //for (int i = 0; i < s2cList.Count; ++i)
            //{
            //    w.WriteLine("{0}DELEGATE_{1}_REGIST_{2}(S2C, CLASS, this)", Util.Tab, infos.mServerNameUP, s2cList[i].cmd_name);
            //}
            //w.WriteLine("}");


            w.WriteLine("*/");
        }

        static private void Write_Header_Packet(StreamWriter w, PacketInfos infos, PacketInfos.Packet p)
        {
            w.Write("{0}void {1}(", Util.Tab, p.cmd_name);
            for (int i = 0; i < p.elems.Count; ++i)
            {
                PacketInfos.Element elem = p.elems[i];
                if (elem.vtype == "string")
                {
                    w.Write("const FString& {0}", elem.vname.ToLower());
                }
                else
                {
                    if (elem.vcount > 0)
                    {
                        w.Write("const {0}* {1}", elem.vtype, elem.vname.ToLower());
                    }
                    else
                    {
                        w.Write("{0} {1}", elem.vtype, elem.vname.ToLower());
                    }
                }

                if (i < p.elems.Count - 1)
                {
                    w.Write(", ");
                }
            }
            w.Write(");");

            w.WriteLine("");
        }
    }
}
