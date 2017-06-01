using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecureServer
{
   public class CardReaderEventReport
    {
       byte[] data;
       public CardReaderEventReport(byte[] data)
       {

           this.data = data;
       }

       public string IP{

           get
           {
               return string.Format("{0}.{1}.{2}.{3}", data[21], data[22], data[23], data[24]);
           }
       }


       public int ID
       {
           get
           {
               return data[0];
           }
       }

       public DateTime TimeStamp
       {

           get
           {
               return new DateTime(2000 + (data[1]>>4)*10+(data[1]&0x0f),
                   ( data[3]>>4)*10+(data[3]&0x0f),
                (data[5] >> 4) * 10 + (data[5] & 0x0f),
                 (data[9] >> 4) * 10 + (data[9] & 0x0f),
                  (data[11] >> 4) * 10 + (data[11] & 0x0f),
                   (data[13] >> 4) * 10 + (data[13] & 0x0f));
           }
       }

       public uint CardNo
       {
           get
           {
               return (uint)(Word0*65536   + Word1) ;
           }
       }


       public int Word0
       {
           get
           {

               return data[14]*256   + data[15] ;
           }

       }

       public int Word1
       {
           get
           {

               return data[16]*256   + data[17] ;
           }

       }

       public int Status
       {
           get
           {
               return data[19];
           }
       }


//       i.	0x00 無動作
//ii.	0x01 開(開鎖)
//iii.	0x02 停
//iv.	0x03 關
//v.	0x04 號碼錯誤
//vi.	0x05 卡號刪除
//vii.	0x06 卡號重複
//viii.	0x07 外部開門
//ix.	0x08 密碼開門
//x.	0x09 ”F1”開門
//xi.	0x0A 反脅迫
//xii.	0x0B 開門超時
//xiii.	0x0C 門開啟
//xiv.	0x0D 門關閉
//xv.	0x0E 卡號連續錯誤

       string[] StatusMapping = new string[]
       {
           "無動作","開鎖","停","關","號碼錯誤","卡號刪除","卡號重複","按鈕開門","密碼開門","F1開門","反脅迫","開門超時",
           "門開啟","門關閉","卡號連續錯誤","自我學習","測試","系統開門","系統關門","異常入侵","外力破壞"
 
       };
      
       public string StatusString {
         
           get{
               return StatusMapping[Status];
             }
       }
       public override string ToString()
       {
           return    TimeStamp.ToLongDateString()+ ","+CardNo+","+StatusString;
       }
    }

   public enum CardReaderStatusEnum : int
   {
       無動作,
       開鎖,
       停,
       關,
       號碼錯誤,
       卡號刪除,
       卡號重複,
       按鈕開門,
       密碼開門,
       F1開門,
       反脅迫,
       門位異常,
       門開啟,
       門關閉,
       卡號連續錯誤,
       自我學習,
       測試,
       系統開門,
       系統關門,
        異常入侵,
       外力破壞
   }
}
