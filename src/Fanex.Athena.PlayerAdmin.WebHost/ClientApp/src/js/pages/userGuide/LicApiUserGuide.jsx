import React from 'react';
import BaseUserGuide from '../../components/BaseUserGuide'

const EngContent = ({isSingleWallet, exportOddsGroup})=> {
    return (<div>
        <div className="rule">
            <h4>General Info</h4>
            <ul>
                <li>This function shows all players information along with their settings.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Search Filters</h4>
            <ul>
                <li><b>Username:</b> searched by Username (Support prefix search).</li>
                <li><b>Currency:</b> select the currency of players that you want to manage.
                    <ul>
                        <li><b>All:</b> all supported currencies will be listed out.</li>
                    </ul>
                </li>
                <li><b>Odds Group:</b> view players of an odds group.</li>
                <li><b>Special Level:</b> view players of a specific special level.</li>
                <li><b>Risk Level:</b> view players of a specific risk level.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Report Guidelines</h4>
            <ul>
                <li><b>Status:</b> view or set status for player.</li>
                <li><b>Username:</b> view player username and number of shortcut options including:
                    <ul>
                        <li>Note</li>
                        <li>Fundtransfer Limit</li>
                        <li>Bet Setting</li>
                        <li>Statement</li>
                    </ul>
                </li>
                {!isSingleWallet && <li><b>Current Balances: </b> indicate available amount to place bets.</li>}
                <li><b>Outstanding Balance:</b> indicate total stakes of all running bets.</li>
                <li><b>Risk Level:</b> view the assigned risk level bet setting and risk level odds group for player.</li>
                <li><b>Odds Group:</b> view or set odds group for player.</li>
                <li><b>Special Level:</b> view or set a special level for player.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Player Details</h4>
            <ul>
                <li><b>Player Info:</b> view player’s profile and activity record.
                    <ul>
                        <li>Player Betting Statistics</li>
                        <li>Fast Cross-Link</li>
                    </ul>
                </li>
                <li><b>Statement: </b> view daily statements of player. All transactions are summarized and then put into a daily record. Each daily record balance will be summed up to the current date as the latest balance.</li>
                <li><b>Detail Transaction:</b> view player balance changes and sort by time.</li>
                <li><b>Bet List:</b> lists out non-settled tickets of player.</li>
                <li><b>Fund Transfer:</b> view transfer log and wallets balance for player.</li>
                <li><b>Bet Setting:</b> view bet setting for Sportsbook.</li>
                <li><b>Note:</b> view or edit the note for player.</li>
                <li><b>Log:</b> view update log of player.</li>
                <li><b>Setting:</b> set fund transfer limit.</li>
            </ul>
            <p>
                <a className="hyper-link" onClick={exportOddsGroup}>User guide for odds group feature</a>
            </p>
        </div>
    </div>)
};

const ZnCNContent = ({isSingleWallet, exportOddsGroup}) => {
    return (<div>
        <div className="rule">
            <h4>一般信息</h4>
            <ul>
                <li>本报表显示所有会员数据包含会员设定。</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>搜索过滤器</h4>
            <ul>
                <li><b>用户名:</b> 搜寻用户名 (支援模糊搜寻)。</li>
                <li><b>货币:</b> 以会员货币来选择报表呈现内容。
                    <ul>
                        <li><b>全部:</b> 报表呈现所有支持货币。</li>
                    </ul>
                </li>
                <li><b>Odds Group:</b> 查看玩家的 odds group。</li>
                <li><b>特殊层级:</b> 查阅特殊层级里的玩家。</li>
                <li><b>风险等级:</b> 查看玩家的风险等级。</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>报告指南</h4>
            <ul>
                <li><b>状态:</b> 查看玩家的状态。</li>
                <li><b>用户名:</b> 查看玩家的用户名和快捷功能选项，包括：
                    <ul>
                        <li>记录</li>
                        <li>转帐额度</li>
                        <li>投注设定</li>
                        <li>帐目</li>
                    </ul>
                </li>
                {!isSingleWallet && <li><b>目前余额:</b> 显示可用于投注的金额。</li>}
                <li><b>未结余额:</b> 显示所有投注中的总投注额。</li>
                <li><b>风险等级:</b> 查看玩家被风控的投注限制和水位组别。</li>
                <li><b>Odds Group:</b> 显示或设定玩家的odds group。</li>
                <li><b>特殊层级:</b> 显示或设定玩家的特殊层级。</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>玩家详情</h4>
            <ul>
                <li><b>玩家资讯:</b> 查看玩家的个人资料和活动记录。
                    <ul>
                        <li>玩家投注统计</li>
                        <li>快速连结</li>
                    </ul>
                </li>
                <li><b>帐目:</b> 查看玩家的每日报表。汇总所有交易，然后记入每日记录。每个每日记录的余额将被累计到当前日期作为最后的余额。</li>
                <li><b>交易记录:</b> 查看玩家余额变化并按时间排序。</li>
                <li><b>投注历史:</b> 列出玩家的未结算注单。</li>
                <li><b>转帐:</b> 查看玩家的转帐日志和钱包余额。</li>
                <li><b>投注设定:</b> 查看体育博彩。</li>
                <li><b>记录:</b> 查看和编辑玩家记录。</li>
                <li><b>日志:</b> 查看玩家的更新日志。</li>
                <li><b>设定:</b> 设置转账限额。</li>
            </ul>
            <p>
                <a className="hyper-link" onClick={exportOddsGroup}>水位组别(Odds Group)用户手册</a>
            </p>
        </div>
    </div>)
};

const ViVNContent = ({isSingleWallet, exportOddsGroup})=> {
    return (<div>
        <div className="rule">
            <h4>Thông Tin Chung</h4>
            <ul>
                <li>Chức năng này hiển thị tất cả thông tin người chơi cùng với cài đặt của họ.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Bộ Lọc Tìm Kiếm</h4>
            <ul>
                <li><b>Tên Người Dùng:</b> tìm kiếm theo tên người dùng (có hỗ trợ đề xuất tìm kiếm).</li>
                <li><b>Tiền Tệ:</b> chọn đơn vị tiền tệ của người chơi mà bạn muốn quản lý.
                    <ul>
                        <li><b>Tất Cả:</b> Liệt kê tất cả đơn vị tiền tệ được hỗ trợ.</li>
                    </ul>
                </li>
                <li><b>Nhóm Tỷ Lệ Cược:</b> xem người chơi của một nhóm tỷ lệ cược.</li>
                <li><b>Cấp Độ Đặc Biệt:</b> xem người chơi ở một cấp độ đặc biệt cụ thể.</li>
                <li><b>Mức Độ Rủi Ro:</b> xem người chơi ở một mức độ rủi ro cụ thể.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Hướng Dẫn Sử Dụng Báo Cáo</h4>
            <ul>
                <li><b>Trạng Thái:</b> xem hoặc đặt trạng thái cho người chơi.</li>
                <li><b>Tên Người Dùng:</b> xem tên người dùng của người chơi và số lượng tùy chọn phím tắt bao gồm:
                    <ul>
                        <li>Ghi Chú</li>
                        <li>Giới Hạn Chuyển Tiền</li>
                        <li>Giới Hạn Cược</li>
                        <li>Sao Kê</li>
                    </ul>
                </li>
                {!isSingleWallet && <li><b>Số Dư Hiện Tại:</b> số tiền khả dụng để đặt cược.</li>}
                <li><b>Tiền Chưa Xử Lý:</b> tổng số tiền cược trên tất cả cược đang diễn ra.</li>
                <li><b>Mức Độ Rủi Ro:</b> xem giới hạn cược và nhóm tỷ lệ cược theo mức độ rủi ro đã được chỉ định cho người chơi.</li>
                <li><b>Nhóm Tỷ Lệ Cược:</b> xem hoặc cài đặt nhóm tỷ lệ cược cho người chơi.</li>
                <li><b>Cấp Độ Đặc Biệt:</b> xem hoặc đặt cấp độ đặc biệt cho người chơi.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Chi Tiết Người Chơi</h4>
            <ul>
                <li><b>Thông Tin Người Chơi:</b> xem hồ sơ người chơi, thông tin ngân hàng và nhật ký hoạt động.
                    <ul>
                        <li>Thống Kê Đặt Cược Của Người Chơi</li>
                        <li>Liên Kết Chéo Nhanh</li>
                    </ul>
                </li>
                <li><b>Sao Kê:</b> xem sao kê theo ngày của người chơi. Tất cả giao dịch được tổng hợp và báo cáo hằng ngày. Mỗi báo cáo số dư hàng ngày sẽ được tích lũy cho đến ngày hiện tại để cho ra số dư mới nhất.</li>
                <li><b>Giao Dịch Chi Tiết:</b> xem biến đổi số dư của người chơi và sắp xếp số dư theo thời gian.</li>
                <li><b>Danh Sách Cược:</b> liệt kê các vé cược chưa kết toán của người chơi.</li>
                <li><b>Chuyển Khoản:</b> xem nhật ký chuyển khoản và số dư ví của người chơi.</li>
                <li><b>Giới Hạn Cược:</b> xem cài đặt giới hạn cược cho Sportsbook.</li>
                <li><b>Ghi Chú:</b> xem hoặc chỉnh sửa ghi chú cho người chơi.</li>
                <li><b>Nhật Ký:</b> xem nhật ký cập nhật của người chơi.</li>
                <li><b>Cài đặt:</b> đặt giới hạn chuyển khoản.</li>
            </ul>
            <p>
                <a className="hyper-link" onClick={exportOddsGroup}>Hướng dẫn sử dụng tính năng nhóm tỷ lệ cược</a>
            </p>
        </div>
    </div>)
};

const IndonesiaContent = ({isSingleWallet, exportOddsGroup})=> {
    return (<div>
        <div className="rule">
            <h4>Informasi Umum</h4>
            <ul>
                <li>Fungsi ini menampilkan informasi semua pemain beserta pengaturan mereka.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Filter Pencarian</h4>
            <ul>
                <li><b>Username:</b> dicari berdasarkan Username (Dukungan pencarian awalan).</li>
                <li><b>Mata Uang:</b> pilih mata uang pemain yang ingin Anda kelola.
                    <ul>
                        <li><b>Semua:</b> semua mata uang yang didukung akan terdaftar.</li>
                    </ul>
                </li>
                <li><b>Grup Odds:</b> melihat pemain dari grup odds tertentu.</li>
                <li><b>Level Khusus:</b> melihat pemain dengan level khusus tertentu.</li>
                <li><b>Level Risiko:</b> melihat pemain dengan level risiko tertentu.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Panduan Laporan</h4>
            <ul>
                <li><b>Status:</b> melihat atau mengatur status pemain.</li>
                <li><b>Username:</b> melihat username pemain dan jumlah opsi pintasan termasuk:
                    <ul>
                        <li>Catatan</li>
                        <li>Batas Transfer Dana</li>
                        <li>Pengaturan Taruhan</li>
                        <li>Rekapan</li>
                    </ul>
                </li>
                {!isSingleWallet && <li><b>Saldo Saat Ini:</b> menunjukkan jumlah yang tersedia untuk melakukan taruhan.</li>}
                <li><b>Saldo Belum Lunas:</b> menunjukkan total taruhan dari semua tiket yang sedang berjalan.</li>
                <li><b>Level Risiko:</b> melihat pengaturan taruhan risiko yang ditetapkan dan level risiko odds.</li>
                <li><b>Grup Komisi:</b> melihat grup komisi untuk pemain.</li>
                <li><b>Level Khusus:</b> melihat atau mengatur level khusus untuk pemain.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>Rincian Pemain</h4>
            <ul>
                <li><b>Info Pemain:</b> melihat profil pemain, informasi bank, dan catatan aktivitas.
                    <ul>
                        <li>Statistik Taruhan Pemain</li>
                        <li>Fast Cross-Link</li>
                    </ul>
                </li>
                <li><b>Rekapan:</b> melihat rekapan harian pemain. Semua transaksi dirangkum dan kemudian dimasukkan ke dalam catatan harian. Setiap saldo catatan harian akan dijumlahkan hingga tanggal saat ini sebagai saldo terbaru.</li>
                <li><b>Transaksi Detail:</b> melihat perubahan saldo pemain dan diurutkan berdasarkan waktu.</li>
                <li><b>Daftar Taruhan:</b> menampilkan tiket yang belum diselesaikan dari pemain.</li>
                <li><b>Transfer Dana:</b> melihat log transfer dan saldo dompet pemain.</li>
                <li><b>Pengaturan Taruhan:</b> melihat pengaturan taruhan untuk Sportsbook.</li>
                <li><b>Catatan:</b> melihat atau mengedit catatan untuk pemain.</li>
                <li><b>Log:</b> melihat log pembaruan pemain.</li>
                <li><b>Pengaturan:</b> batas transfer dana.</li>
            </ul>
            <p>
                <a className="hyper-link" onClick={exportOddsGroup}>Panduan pengguna untuk fitur grup odds</a>
            </p>
        </div>
    </div>)
};


const ThailandContent = ({isSingleWallet, exportOddsGroup})=> {
    return (<div>
        <div className="rule">
            <h4>ข้อมูลทั่วไป</h4>
            <ul>
                <li>ฟังก์ชั่นนี้แสดงข้อมูลผู้เล่นทั้งหมดพร้อมกับการตั้งค่า.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>ตัวกรองการค้นหา</h4>
            <ul>
                <li><b>ชื่อผู้ใช้:</b> ค้นหาโดยชื่อผู้ใช้ (รองรับการค้นหาคำนำหน้า).</li>
                <li><b>สกุลเงิน:</b> เลือกสกุลเงินของผู้เล่นที่คุณต้องการจัดการ.
                    <ul>
                        <li><b>ทั้งหมด:</b> สกุลเงินที่รองรับทั้งหมดจะแสดงออกมา.</li>
                    </ul>
                </li>
                <li><b>กลุ่มอัตราต่อรอง:</b> ดูผู้เล่นของกลุ่มอัตราต่อรอง.</li>
                <li><b>ระดับพิเศษ:</b> ดูผู้เล่นในระดับพิเศษเฉพาะ.</li>
                <li><b>ระดับความเสี่ยง:</b> ดูผู้เล่นในระดับความเสี่ยงเฉพาะ.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>ตัวชี้การรายงาน</h4>
            <ul>
                <li><b>สถานะ:</b> ดูหรือตั้งค่าสถานะของผู้เล่น.</li>
                <li><b>ชื่อผู้ใช้:</b> ดูชื่อผู้ใช้ของผู้เล่นและจำนวนตัวเลือกทางลัดรวมถึง:
                    <ul>
                        <li>หมายเหตุ</li>
                        <li>วงเงินการโอน</li>
                        <li>การตั้งค่าเดิมพัน</li>
                        <li>รายงานการเงิน</li>
                    </ul>
                </li>
                {!isSingleWallet && <li><b>ยอดคงเหลือปัจจุบัน:</b> ระบุจำนวนเงินที่สามารถวางเดิมพันได้.</li>}
                <li><b>ยอดคงค้าง:</b> ระบุเงินเดิมพันรวมของการเดิมพันที่กำลังดำเนินอยู่ทั้งหมด.</li>
                <li><b>ระดับความเสี่ยง:</b> ดูตั้งค่าการเดิมพันระดับความเสี่ยงที่กำหนดและอัตราต่อรองระดับความเสี่ยง.</li>
                <li><b>กลุ่มอัตราต่อรอง:</b> ดูหรือกำหนดกลุ่มอัตราต่อรองของผู้เล่น.</li>
                <li><b>ระดับพิเศษ:</b> ดูหรือตั้งค่าระดับพิเศษของผู้เล่น.</li>
            </ul>
        </div>
        <div className="line" />
        <div className="rule">
            <h4>รายละเอียดผู้เล่น</h4>
            <ul>
                <li><b>ข้อมูลผู้เล่น:</b> ดูโปรไฟล์ของผู้เล่น ข้อมูลธนาคาร และบันทึกกิจกรรม.
                    <ul>
                        <li>สถิติการเดิมพันของผู้เล่น</li>
                        <li>เชื่อมโยงอย่างเร็ว</li>
                    </ul>
                </li>
                <li><b>รายงานการเงิน:</b> ดูรายงานการเงินประจำวันของผู้เล่น สรุปรายการทั้งหมดแล้วลงบันทึกประจำวัน ยอดบันทึกรายวันแต่ละรายการจะถูกรวมเข้ากับวันที่ปัจจุบันเป็นยอดล่าสุด.</li>
                <li><b>รายละเอียดการทำธุรกรรม:</b> ดูการเปลี่ยนแปลงยอดคงเหลือของผู้เล่นและจัดเรียงตามเวลา.</li>
                <li><b>รายการเดิมพัน:</b> แสดงรายการตั๋วที่ยังไม่ตัดสินของผู้เล่น.</li>
                <li><b>การโอนเงิน:</b> ดูบันทึกการโอนและยอดเงินคงเหลือของผู้เล่น.</li>
                <li><b>ตั้งค่าการเดิมพัน:</b> ดูการตั้งค่าการเดิมพันสำหรับ สปอร์ตบุ๊ค.</li>
                <li><b>หมายเหตุ:</b> ดูหรือแก้ไขบันทึกของผู้เล่น.</li>
                <li><b>บันทึก:</b> ดูบันทึกวันที่ของผู้เล่น.</li>
                <li><b>ตั้งค่า:</b> วงเงินโอน.</li>
            </ul>
            <p>
                <a className="hyper-link" onClick={exportOddsGroup}>คู่มือผู้ใช้สำหรับคุณสมบัติกลุ่มอัตราต่อรอง</a>
            </p>
        </div>
    </div>)
};

const Contents = {
    engContent: props => <EngContent {...props} />,
    znCNContent: props => <ZnCNContent {...props} />,
    viVNContent: props => <ViVNContent {...props} />,
    thTHContent: props => <ThailandContent {...props} />,
    idIDContent: props => <IndonesiaContent {...props} />,
};

const LicApiUserGuide = props => {
    const { lang, isSingleWallet, exportOddsGroup } = props;
    return (<React.Fragment>
        <BaseUserGuide lang={lang} isSingleWallet={isSingleWallet} exportOddsGroup={exportOddsGroup} contents={Contents} />
    </React.Fragment>);
}

export default LicApiUserGuide;