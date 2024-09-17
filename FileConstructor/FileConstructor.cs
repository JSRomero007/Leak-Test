using System;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.IO;


namespace LeakInterface.Global
{
    public class FileConstructor
    {
        public void WriteToFileIfEmpty(string path, string content)
        {
            // Verifica si el archivo existe y está vacío antes de escribir
            if (File.Exists(path) && new FileInfo(path).Length == 0)
            {
                File.WriteAllText(path, content);
            }
        }


        public void FileLog(string path)
        {
            string Content= " this date:"+ DateTime.Now;

        }

        ///------------------- DefaultConfig -------------------\\\
        public void DefaultConfig(string path) 
        {
                             string Cont=
                                             "[CardConfig]" +
                                          "\r\nDweel_S1= 1" +
                                          "\r\nThreshold_S1 =1 " +
                                          "\r\nSettle_S1 = 2" +
                                          "\r\nMin_S1 =3" +
                                          "\r\nTime_S1 =255" +
                                          "\r\nFinal_S1 =255" +
                                          "\r\nDweel_S2= 1" +
                                          "\r\nThreshold_S2 =1" +
                                          "\r\nSettle_S2 = 2" +
                                          "\r\nMin_S2 =3" +
                                          "\r\nTime_S2 =255" +
                                          "\r\nFinal_S2 =255";

            WriteToFileIfEmpty(path,Cont);
        }

        ///------------------- PortConfig -------------------\\\
        public void PortConfig(string path)
        {                      string Cont =
                                             "[ConfigNodes]" +
                                          "\r\nPortCom=COM0" +
                                          "\r\nNodesUse=2";
            WriteToFileIfEmpty(path,Cont);  
        }
        ///------------------- ScreenConfig -------------------\\\
        public void ScreenConfig(string path)
        {                      string Cont = 
                                            "[ScreenConfig]" +
                                          "\r\nScreen=0";
            WriteToFileIfEmpty(path, Cont);
        }

        ///------------------- ActiveSequence -------------------\\\
        public void ActiveSequence(string path)
        {
            string Cont=  
                                               "[Config]" +
                                           "\r\nAutoleak = Enable" +
                                           "\r\nNameSec = " +
                                           "\r\nIdSec = " +
                                           "\r\nZcode = " +
                                           "\r\nProgrammer = " +
                                           "\r\nDateSec = " +
                                           "\r\nVersion = " +
                                           "\r\nNodeNum = ";
            WriteToFileIfEmpty(path, Cont);
        }


        ///------------------- UserConfig -------------------\\\
        public void UserConfig(string path)
        {
            string Cont =
                                              "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                          "\r\n<Main>" +
                                          "\r\n  <User id=\"0\">" +
                                          "\r\n    <Name>Jose Israel Cosme Romero</Name>" +
                                          "\r\n    <Area>Development</Area>" +
                                          "\r\n    <Code>Key1004098ws</Code>" +
                                          "\r\n    <Level>Aptiv-Develop-UEL</Level>" +
                                          "\r\n  </User>" +
                                          "\r\n  <User id=\"1\">" +
                                          "\r\n    <Name>JICR</Name>" +
                                          "\r\n    <Area>Development111</Area>" +
                                          "\r\n    <Code>Key1004098ws22</Code>" +
                                          "\r\n    <Level>Aptiv-Develop-UEL</Level>" +
                                          "\r\n  </User>" +
                                          "\r\n  <User id=\"2\">" +
                                          "\r\n    <Name>Admin</Name>" +
                                          "\r\n    <Area>Admin</Area>" +
                                          "\r\n    <Code>Aptiv1004098</Code>" +
                                          "\r\n    <Level>Aptiv-Admin-log</Level>" +
                                          "\r\n  </User>" +
                                          "\r\n</Main>";
            WriteToFileIfEmpty(path,Cont);
        }
        ///------------------- DBSequences -------------------\\\
        public void DBSequences(string path)
        {
            string Cont =
                                              "ID Owner,Date,Hour,Z Code,Version Sequence,Name Sequence,Test Values,Num Holders,Sequence" +
                                          "\r\nN/A,Test,May 06,09:39 AM,ZXXXX0000,0.0,Example_01,S1-1_1_2_3_255_255,2,|1S1 A-0 1 1 1 1 1 1|2S1 A-0 1 1 1 1 1 1" +
                                          "\r\nN/A,Test,May 06,09:43 AM,ZXXXX0001,0.0,Example_02,S1-1_1_2_3_255_255,4,|1S1 A-0 1 1 1 1 1 1|1S2 A-1 1 1 2 3 255 255|2S1 B-0 1 1 1 1 1 1|2S2 B-1 1 1 2 3 255 255\r\n";
            WriteToFileIfEmpty(path,Cont);
        }
        ///------------------- GeneryLog -------------------\\\
        public void GeneryLog(string path)
        {
            string Cont =

                                              "[First Config]" +
                                          "\r\n- Log created - Status OK";

            WriteToFileIfEmpty(path, Cont);
        }


        ///------------------- Weetech -------------------\\\
        public void Weetech(string path)
        {
            string Cont =
                                              "[Config]" +
                                          "\r\nZCode = Z12345111" +
                                          "\r\nState = Start" +
                                          "\r\nPass = True" +
                                          "\r\nError = ";
            WriteToFileIfEmpty(path, Cont);
        }
        ///------------------- CSequences -------------------\\\
        public void CSequences(string path)
        {
            string Cont =
                                             "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                         "\r\n<Main>" +
                                         "\r\n  <SequenceUsed>" +
                                         "\r\n    <Autoleak>Enable</Autoleak>" +
                                         "\r\n    <NameSec> 11111</NameSec>" +
                                         "\r\n    <idSec>1</idSec>" +
                                         "\r\n    <Zcode> V001</Zcode>" +
                                         "\r\n    <Programmer> JICR</Programmer>" +
                                         "\r\n    <DateSec> September 19</DateSec>" +
                                         "\r\n    <version> V001</version>" +
                                         "\r\n    <NodeNum> 6</NodeNum>" +
                                         "\r\n    <SensorActive>3</SensorActive>" +
                                         "\r\n  </SequenceUsed>" +
                                         "\r\n</Main>";
            WriteToFileIfEmpty(path, Cont);
        }
    
        }
}
