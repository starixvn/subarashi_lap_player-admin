export default class PlayerListDataHelper {
    static mergePlayerList(lrfList, lapList) {
        if (!lrfList || !lapList) {
            return [];
        }
        const lrfDic = this.toLrfPlayerDic(lrfList);
        return lapList.reduce((result, lapItem) => {
            const lrfItem = lrfDic[lapItem.IBCCustId] || {};
            const playerItem = this.mergePlayerItem(lrfItem, lapItem);
            result.push(playerItem);

            return result;
        }, []);
    }

    static mergePlayerItem(lrfItem, lapItem) {
        const { OddsGroup, BaStatus, SpecialLevel } = lrfItem;
        const {
            Status, RealName, email, isOnline, IBCCustId, isArchived, LastTransaction,
            CurrencyCode, OutBalance, IBCUserName, Vender_Member_ID, CreatedDateString
        } = lapItem;

        return {
            IbcCustId: IBCCustId,
            LicenseeCustId: lapItem.CustId,
            UserName: lapItem.UserName,
            SystemId: IBCUserName,
            VendorMemberId: Vender_Member_ID,
            Currency: CurrencyCode,
            CurrentBalance: lapItem.CurrentBalance,
            OutstandingBalance: OutBalance,
            OddsGroup,
            BaStatus,
            SpecialLevel,
            Status,
            RealName,
            LastTransaction,
            Email: email,
            IsOnline: isOnline,
            IsDeposit: IBCCustId > 0,
            IsArchived: !!isArchived,
            CurrencyId: lapItem.Currency,
            CreatedDate: CreatedDateString,
            CommissionGroup: lrfItem.CommissionGroup,
            CustomerClass: lrfItem.CustomerClass
        };
    }


    static toLrfPlayerDic(lrfList) {
        const lrfPlayerDic = lrfList.reduce((result, currentItem) => {
            result[currentItem.IbcCustId] = currentItem;
            return result;
        }, {});
        return lrfPlayerDic;
    }

    static createEmptyResult() {
        return {
            playerList: [],
            totalItem: 0
        };
    }
}
