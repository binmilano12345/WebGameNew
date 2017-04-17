using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppConfig;
using Us.Mobile.Utilites;

public class PanelHelp : MonoBehaviour
{
	[SerializeField]
	Text txt_id, txt_money;
	[SerializeField]
	Text txt_hotro;
	[SerializeField]
	Text txt_content_luat;

	#region LUAT CHOI
	static string tlmn = "* Rác : là những lá bài riêng lẻ không thể kết hợp với lá bài còn lại. Một lá bài rác chỉ có thể được mang ra đánh thắng một lá bài rác khác khi nó có giá trị cao hơn lá bài kia, hoặc cùng giá trị với bài kia nhưng có nước bài cao hơn\n* Đôi : 2 quân bài có cùng giá trị như nhau. Đôi bài đánh thắng một đôi bài khác khi nó có giá trị cao hơn đôi bài kia, hoặc cùng giá trị với đôi bài kia nhưng có nước bài cao hơn.\n* Sám cô (Bộ ba) : 3 quân bài cùng giá trị như nhau. Sám cô có thể đánh thắng một Sám cô khác khi nó có giá trị cao hơn Sám cô kia, hoặc cùng giá trị với Sám cô kia nhưng có nước bài cao hơn.\n* Sảnh: 3 quân bài hay nhiều quân bài hợp thành một dãy số liên tiếp. Một Sảnh có thể đánh thắng một Sảnh khác khi nó có giá trị cao hơn Sảnh kia, hoặc cùng giá trị với Sảnh kia nhưng có nước bài cao hơn.\n* Tập hợp đặc biệt: Tứ quý :4 quân bài cùng giá trị như nhau. Một Tứ quý có thể đánh thắng một Tứ quý khác khi nó có giá trị cao hơn Tứ quý kia.\n* 3Đôi thông : 6 quân bài hợp thành một dãy liên tiếp của 3 đôi. 3 Đôi thông có thể đánh thắng 3 Đôi thông khác khi nó có giá trị cao hơn 3 Đôi thông kia, hoặc cùng giá trị với 3 Đôi thông kia nhưng có nước bài cao hơn.\n* 4 Đôi thông: 8 quân bài hợp thành một dãy liên tiếp của 4 đôi.Cách thắng cũng giống như 3 Đôi thông. 5 Đôi thông: 10 quân bài hợp thành một dãy liên tiếp của 5 đôi.Cách thắng cũng giống như 3 Đôi thông. 6 Đôi thông: 12 quân bài hợp thành một dãy liên tiếp của 6 đôi. Cách thắng cũng giống như 3 Đôi thông.\n* Những kết hợp đặc biệt này có khả năng đánh thắng (thường gọi là chặt lá bài(hay kết hợp) lớn nhất là 2 (thường gọi Heo):\n* 3 Đôi thông được chặt một heo, và chặt chồng lên 3 đôi thông nhỏ hơn nó (nhưng phải theo vòng chơi).\n* Tứ quý được chặt một heo hoặc một đôi heo, chặt chồng lên 3 đôi thông bất kì, chặt chồng lên tứ quý nhỏ hơn nó (nhưng phải theo vòng chơi).\n* 4 Đôi thông được chặt một heo hoặc một đôi heo, chặt chồng lên 3 đôi thông, chặt chồng lên tứ quý, chặt chồng lên 4 đôi thông nhỏ hơn nó (tự do, không phải theo vòng chơi).\n* Chặt chồng cuối cùng là tổng kết tất cả các hành vi chặt trước đó. Người bị chặt sau cùng sẽ phải chịu toàn bộ tiền chặt.\n"
	              + " CÁC TRƯỜNG HỢP TỚI TRẮNG\n"
	              + "* Nếu một người có bộ Sảnh Rồng, 5 đôi thông liền nhau, 6 đôi thường, tứ quý 2 thì được xử Tới Trắng, Những người còn lại phải trả số  tương ứng với số lá là 13x2 = 26 lá (nhưng không tính thối hai và các trường hợp đặc biệt)\n"
	              + "* Nếu một người đánh hết 13 lá bài của mình mà những người còn lại chưa đánh được lá bài nào thì người đó được xử Tới Trắng, Những người còn lại phải trả số  tương ứng với số lá là 13x2 = 26 lá (đồng thời tính thối hai và các trường hợp đặc biệt)\n"
	              + "* Ở ván bài đầu tiên của một bàn/ phòng, nếu người chơi có Tứ Quý 3 tính thắng trắng \n"
	              + "* Nếu có nhiều trường hợp Tới Trắng ở ván đầu tiên thì người nào có 3 Bích sẽ được xử thắng. Trong các ván khác, người nào có lượt đánh trước sẽ được xử thắng\n"
	              + " TỈ LỆ CỘNG TRỪ TIỀN\n"
	              + "* Căn cứ vào số lá bài còn lại trên tay mỗi người khi có người về nhất hoặc tới trắng.\n"
	              + "* Số lá bài bị phạt được tính bởi số lá bài bình thường và số lá bài đặc biệt quy đổi ra.\n"
	              + "* Cách quy đổi lá bài đặc biệt ra lá bài bình thường như sau:\n"
	              + " - 3 Bích           5 lá bài bình thường\n"
	              + " - 2 Đen            5 lá bài bình thường\n"
	              + " - 2 Đỏ             10 lá bài bình thường\n"
	              + " - 3 Đôi Thông      10 lá bài bình thường\n"
	              + " - Tứ Quý           20 lá bài bình thường\n"
	              + " - 4 Đôi Thông      30 lá bài bình thường\n"
	              + " * Nếu người chơi đánh 3 bích cuối cùng để về nhất thì nhân 3 số lá bài trên tay của người chơi +  số lá bài của các trường hợp đặc biệt.\n";
	static string phom = "* Một người có 10 quân và những người còn lại nhận 9 quân bài. Phần còn lại của bộ bài đặt vào giữa bàn\n* Người đi đầu (người có 10 quân) bỏ đi 1 lá bài rác trên tay của mình. Người kế tiếp có thể ăn lá bài đó nếu nó có thể hợp với bài trên tay thành một phỏm.\n* Nếu người kế tiếp không thể ăn hay không muốn ăn lá bài rác người tay trên đánh xuống, người đó phải nhận thêm 1 lá bài từ bộ bài ở giữa bàn.\n* Ván bài kết thúc khi có một người ù (Số lá bài trên tay người chơi có thể sắp xếp thành phỏm và không thừa lá bài nào, hoặc tương đương với sau khi hạ hết phỏm người đó còn 0 điểm).\n* Nếu không có ai ù, ván bài sẽ kết thúc sau 4 vòng đánh. Trước khi vứt đi lá bài rác trong vòng 4, người chơi cần trình tất cả những phỏm mình có cho mọi người biết.";
	static string poker = "- Bắt đầu mỗi ván bài, mỗi người chơi sẽ được chia 2 lá bài, người chơi chỉ có thể thấy 2 lá bài của mình, chúng được gọi là hole card.\n- Sau đó lần lượt 5 lá bài tiếp theo sẽ được chia làm ba đợt và hiển thị trên bàn, 5 lá này đều được mở ra và mọi người đều có thể thấy nó, và chúng được gọi là community card. Đợt chia bài community cards đầu tiên sẽ là 3 lá, đợt thứ 2 và đợt chia cuối cùng, mỗi đợt là 1 lá. Và qua mỗi đợt đó, người chơi sẽ có quyền đặt cược, như vậy tính cả vòng đầu tiên, khi chỉ mới có 2 lá bài, là tổng cộng sẽ có 4 vòng cược cho các người chơi, các vòng này được gọi là betting round.";
	static string xito = "Game sử dụng bộ bài 52 lá (từ bộ tứ 7 đến bộ tứ A).\n+ Thứ tự đánh bài ngược chiều kim đồng hồ\n+ Sau khi bắt đầu, bài sẽ được chia cho mỗi người trong phòng 2 cây và mỗi người sẽ được quyền lựa chọn 1 lá bài ngửa ra, còn 1 lá úp xuống\n+ Tố tất cả: cược tất cả tiền bạn có. Số tiền này không vượt quá mức cược tối đa.\n+ Theo: Khi bạn nhấn Theo, bạn sẽ cược thêm số tiền bằng với số tiền người trước đã Tố.\n+ Úp bỏ: Trong trường hợp bài của bạn quá yếu so với đối phương hoặc bạn muốn bỏ cuộc, bạn có thể sử dụng nút Úp bỏ. Lúc này bạn sẽ bị mất số gold mà mình đã đặt cược.\n+ Sau khi mỗi người đã có đủ 5 lá, người có bộ bài lớn nhất sẽ là người thắng\n+ Trong trường hợp hai người chơi có bộ bài giống hệt nhau về số, trật tự chất của bài Bridge sẽ quyết định: Bích > Cơ > Rô > Tép/Chuồn";
	static string lieng = "- Phải có ít nhất từ 2 người chơi để tham gia chơi Liêng. Trước khi chia bài, tất cả người chơi sẽ bỏ ra một số tiền bằng nhau (Tiền sàn).\n- Mỗi người chơi sẽ được chia 3 lá và ba lá bài này đối thủ không hề biết trước.\n- Sau đó người chơi bắt đầu đặt cược. Chỉ có một vòng cược duy nhất.\n- Sau khi mọi người đặt tiền cược, người nào có bộ bài mang giá trị cao nhất sẽ là người thắng cuộc.\n"
	               + "Ngoài việc ăn khi tố bình thường nếu người chơi lên sáp thì mỗi nhà phải trả cho người lên sáp gấp 3 lần số tiền bàn cược ";
	static string bacay = "- Bộ bài Game 3 cây Online gồm 36 cây sau khi loại bỏ các quân bài 10, J(bồi), Q(đầm), K(già).\n- Số lượng người chơi giới hạn: từ 2 đến 12 người.\n- Sau khi người chơi đặt cược, ván bài được bắt đầu.\n- Mỗi người chơi được chia lần lượt 3 quân bài ngẫu nhiên, tính tổng điểm 3 lá bài và trừ đi bội số của 10 (lấy phân lẻ) làm kết quả so sánh.\n- Nếu điểm bằng nhau sẽ so sánh từng quân bài của hai bên. Trước tiên là so theo chất theo thứ tự Rô, Cơ, Tép, Bích. Sau đó so theo con số của chất, lớn ăn bé. Trường hợp đặc biệt, bài có Át rô là bài có chất cao nhất\n"
	               + "Đánh tới : Nếu người chơi có bài tổng điểm là 8, 9, 10 sẽ được hoặc thua 1 số tiền gấp đôi số tiền đã đặt. \n"
	               + "- Mười Át rô nhân 3 số tiền đã đặt.\n" + "- Sáp nhân 4 số tiền đã đặt. ";
	static string maubinh = "- Người chơi có 60s để sắp xếp bài của mình trước khi đọ bài với những người chơi còn lại trong ván chơi đánh bài mậu binh.\n- Mỗi người chơi sẽ được chia 13 lá bài, được chia thành 3 chi. Chi đầu và giữa gồm 5 lá bài. Chi cuối gồm 3 lá bài.\n- Yêu cầu người chơi đánh bài mậu binh phải sắp xếp sao cho chi trước mạnh hơn chi sau. Nếu người chơi đánh bài không tuân thủ đúng quy tắc rất dễ bị hệ thống đánh dấu là Binh Lủng tương đương với ván bài sập làng.";
	static string sam = "\n1. Sơ lược về game:\nSâm là game bài dành cho 2-4 người chơi. Mỗi người sẽ được chia 10 lá bài. Ván bài kết thúc khi có một người hết bài, người thắng ăn hết tiền của những người còn lại.\nTrong game Sâm bộ bài không phân biệt chất Cơ, Rô, Tép, Bích.\nBộ bài có giá trị bé đến lớn như sau: 3, 4, 5, 6, 7, 8, 9, 10, J, Q, K, A, 2. 3 là bé nhất, 2 là lớn nhất.\n\nCác bộ bài trong game Sâm:\n-Đôi: 2 quân bài cùng giá trị: 33, 44, 77\n-Xám cô:3 cây cùng giá trị: 777\n-Sảnh: Có 3 cây trở lên có giá trị liên tiếp: 345, 5678\n-Tứ quý: 4 cây cùng giá trị\nHệ thống sẽ chọn ngẫu nhiên người đánh trước, nếu không có ai báo sâm. Từ ván thứ 2 trở đi, người thắng ván trước sẽ được đánh trước. Lượt chơi theo ngược chiều kim đồng hồ. Bắt đầu ván bài người chơi được quyền báo Sâm.\n\n2. Cách đánh\n\nNhững ngươi chơi khác sẽ chặt để cướp lượt hoặc thôi bỏ lượt. Khi người chơi đánh ra 1 lá bài/1 bộ bài, người chơi tiếp theo sẽ phải chặt bằng 1 lá bài/1 bộ bài có giá trị mạnh hơn. Người nào thắng ở lượt đó sẽ bắt đầu đánh lượt mới. Người chơi đã bỏ lượt có thể đánh tiếp nếu chặt được 2.\nNgười hết bài đầu tiên sẽ thắng.\n\n3. Luật chặt\nCác loại bài giá trị lớn hơn, có thể chặt các loại bài nhỏ hơn\nSảnh QKA,JQKA là lớn nhất. Không có sảnh KA2, QKA2,...\nTrường hợp đặc biệt: Tứ quý>2\n\n4. Báo Sâm\nSau khi chia bài người chơi có thế lựa chọn báo Sâm. Người báo Sâm nhanh nhất sẽ được đánh đầu tiên.\nNgười báo Sâm, nếu có thể đánh hết lá bài mà không bị ai chặn thì sẽ báo Sâm thành công.\nNếu người báo Sâm bị chặn trước khi hết bài thì sẽ báo Sâm không thành công.\nNếu không báo Sâm thì lượt chơi sẽ bắt đầu từ nhà cái như bình thường."
	             + "* Tỉ Lệ Tính Tiền Các Cước Đặc Biệt Như Sau:\n"
	             + " - Tứ quý chặt 2 = 10 lá \n"
	             + " - Thối 2  nhân 10 lá + số cây bài bình thường trên tay  \n"
	             + " - Thối tứ quý nhân 20 lá + số cây bài bình thường trên tay\n";
	static string _taixiu = "Bước 1: Chọn nút Dự đoán Tài hoặc Xỉu*\nBước 2: Nhập số "
	                 + GameConfig.MONEY_UNIT_VIP
	                 + " muốn dự đoán\nBước 3: Chọn nút Đồng ý để xác nhận**\nBước 4: Theo dõi kết quả để biết Thắng hay Thua***\n\n(*)Tài  =>11 điểm, Xỉu=<10 điểm\n(**)Cách chơi game:\n+ Tham gia dự đoán phiên chơi về tài hoặc xỉu\n+ Khi kết thúc thời gian lựa chọn, nếu số "
	                 + GameConfig.MONEY_UNIT_VIP
	                 + " hai bên chênh lệch, số chênh lệch sẽ được hoàn trả lại cho người chơi (ưu tiên người chơi dự đoán sớm hơn)\n+Người chơi dự đoán tối thiểu là 50"
	                 + GameConfig.MONEY_UNIT_VIP
	                 + " và tối đa là số dư tài khoản\n(***)Trả thưởng khi có kết quả \n+ Hệ thống tính phí 2% số "
	                 + GameConfig.MONEY_UNIT_VIP
	                 + " dựa trên tiền thắng\n+ Có thể chọn nút (!) để xem lịch sử giao dịch\nVD: Khi game có 1000 "
	                 + GameConfig.MONEY_UNIT_VIP + " bên Tài  - 900 " + GameConfig.MONEY_UNIT_VIP + " bên Xỉu A chọn 200 " + GameConfig.MONEY_UNIT_VIP
	                 + " bên Xỉu, sau đó B chọn 200 " + GameConfig.MONEY_UNIT_VIP
	                 + " bên Xỉu -> hết giờ chọn cược hệ thống trả lại 200 " + GameConfig.MONEY_UNIT_VIP + " cho bên B, 100 "
	                 + GameConfig.MONEY_UNIT_VIP + " cho bên A (để tổng " + GameConfig.MONEY_UNIT_VIP + " hai bên đạt 1000 " + GameConfig.MONEY_UNIT_VIP
	                 + " ). Kết quả bên Xỉu -> A thắng nhận được (100x2) - (100x2%) = 198" + GameConfig.MONEY_UNIT_VIP + ")\n";
	static string xocdia = " Luật chơi xóc đĩa:\n"
	                + "1. Hệ thống làm nhà cái:\n"
	                + "Người chơi vào bàn, nhà cái sẽ xóc đĩa, trong khoảng thời gian 20s người chơi có thể đặt cược vào các cửa:\n"
	                + "+ Chẵn (1x1)\n"
	                + "+ Lẻ (1x1)\n"
	                + "+ Tứ chẵn (1x10)\n"
	                + "+ Tứ lẻ (1x10)\n"
	                + "+ 3 trắng 1 đỏ (1x3)\n"
	                + "+ 3 đỏ 1 trắng (1x3).\n"
	                + "- Thế nào là chẵn: Kết quả có 2 vị (xúc xắc) úp và 2 vị ngửa hoặc 4 vị úp hoặc 4 vị ngửa. Có thể phân biệt úp, ngửa bằng màu sắc của con xúc xắc.\n"
	                + "- Thế nào là lẻ: Kết quả có 1 con úp, 3 con ngửa hoặc 1 con ngửa, 3 con úp.\n"
	                + "2. Người chơi làm nhà cái:\n"
	                + "- 20s đầu tiên: hệ thống sẽ xóc dĩa, để cho người chơi đặt cược, hủy cược, đặt gấp đôi, chọn mức cược.\n"
	                + "- 10s tiếp theo là thời gian dành cho người chơi là nhà cái có thể chọn 1 trong các lựa chọn sau: bán cửa chẵn, bán cửa lẻ, cân hết.\n"
	                + "Như nào gọi là bán cửa chẵn, bán cửa lẻ, cân hết???\n"
	                + "+ Bán cửa chẵn/lẻ là: nhà cái bán hết cửa lẻ để trả lại tiền cho người chơi, không ôm cửa đó nữa. Trong trường hợp về cửa đó cũng không được ăn.\n"
	                + "+ Cân hết: nhà cái cân hết nghĩa là: số tiền cược của người chơi cùng đặt vào cửa nào không về nhà cái sẽ được thu hết, số tiền ở cửa thắng thì trả cho người chơi.\n"
	                + "- Trường hợp tổng số tiền thắng/thua trong ván vượt quá số tiền của nhà cái đang có thì nhà cái chỉ được nhận/mất đi số tiền mà nhà cái đang có. Số tiền còn lại hệ thống sẽ tự cân.\n"
	                + "- Khi số tiền của người chơi làm nhà cái nhỏ hơn số tiền min để làm nhà cái, thì người chơi đó sẽ không được làm nhà cái nữa và nhận được alert thông báo: “bạn không đủ tiền để làm nhà cái, cần ít nhất xxx tiền để làm nhà cái của bàn cược xxx "
	                + GameConfig.MONEY_UNIT_VIP;

	#endregion

	[SerializeField]
	Text[] txt_tab;

	[SerializeField]
	Toggle[] tg_tab;
	static	string[] luatchoi = new string[]{ _taixiu, xocdia, tlmn, sam, maubinh, phom, lieng, poker, xito, bacay };
	// Use this for initialization
	void Start ()
	{
		txt_hotro.text = string.Format (ClientConfig.Language.GetText ("helper")
			, GameConfig.HOT_LINE
			, GameConfig.FANPAGE
			, GameConfig.MAIL_HELPER);
		txt_tab [0].text = "Tài Xỉu";
		txt_tab [1].text = "Xóc Đĩa";
		txt_tab [2].text = "Tiến Lên";
		txt_tab [3].text = "Sâm";
		txt_tab [4].text = "Mậu Binh";
		txt_tab [5].text = "Phỏm";
		txt_tab [6].text = "Liêng";
		txt_tab [7].text = "Poker";
		txt_tab [8].text = "Xì Tố";
		txt_tab [9].text = "Ba Cây";
		for (int i = 0; i < tg_tab.Length; i++) {
			Toggle tg = tg_tab[i];
			tg.name = i + "";
			tg.onValueChanged.AddListener (delegate {
				OnValueChange (tg);
			});
		}
		OnValueChange (tg_tab[0]);
	}

	void OnEnable(){
		txt_id.text = "ID: " + ClientConfig.UserInfo.USER_ID;
		txt_money.text = MoneyHelper.FormatMoneyNormal (ClientConfig.UserInfo.CASH_FREE);
	}

	void OnValueChange (Toggle obj)
	{
		if (obj.isOn) {
			int index = int.Parse (obj.name);
			txt_content_luat.text = luatchoi [index];
		}
	}
}
