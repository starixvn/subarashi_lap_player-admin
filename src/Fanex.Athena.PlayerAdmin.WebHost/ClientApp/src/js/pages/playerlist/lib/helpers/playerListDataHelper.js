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

    static mergePlayerListBasedLrf(lrfList, lapList) {
        if (!lrfList || !lapList) {
            return [];
        }
        const lapDic = this.toLapPlayerDic(lapList);
        return lrfList.reduce((result, lrfItem) => {
            const lapItem = lapDic[lrfItem.IbcCustId] || {};
            const playerItem = this.mergePlayerItem(lrfItem, lapItem);
            result.push(playerItem);

            return result;
        }, []);
    }

    static mergePlayerItem(lrfItem, lapItem) {
        const IbcCustId = lapItem.IBCCustId || lrfItem.IbcCustId || 0;
        return {
            IbcCustId,
            LicenseeCustId: lapItem.CustId || 0,
            UserName: lapItem.UserName || lrfItem.UserName,
            SystemId: lapItem.IBCUserName || lrfItem.SystemId,
            VendorMemberId: lapItem.Vender_Member_ID,
            Currency: lapItem.CurrencyCode || lrfItem.Currency,
            CurrentBalance: lapItem.CurrentBalance || lrfItem.CurrentBalance,
            OutstandingBalance: lapItem.OutBalance || lrfItem.OutstandingBalance,
            OddsGroup: lrfItem.OddsGroup,
            BaStatus: lrfItem.BaStatus,
            SpecialLevel: lrfItem.SpecialLevel,
            Status: lapItem.Status,
            RealName: lapItem.RealName,
            LastTransaction: lapItem.LastTransaction,
            Email: lapItem.email,
            IsOnline: !!lapItem.isOnline,
            IsDeposit: IbcCustId > 0,
            IsArchived: !!lapItem.isArchived,
            CurrencyId: lapItem.Currency || lrfItem.CurrencyId,
            CreatedDate: lapItem.CreatedDateString,
            CommissionGroup: lrfItem.CommissionGroup,
            CustomerClass: lrfItem.CustomerClass,
            RiskLevel: lrfItem.RiskLevel,
            RiskLevelKey: lrfItem.RiskLevelKey
        };
    }


    static toLrfPlayerDic(lrfList) {
        return this._toPlayerDic(lrfList, 'IbcCustId');
    }

    static toLapPlayerDic(lapList) {
        return this._toPlayerDic(lapList, 'IBCCustId');
    }

    static _toPlayerDic(playerList, custIdPropertyName) {
        const playerListDic = playerList.reduce((result, currentItem) => {
            result[currentItem[custIdPropertyName]] = currentItem;
            return result;
        }, {});
        return playerListDic;
    }

    static createEmptyResult() {
        return {
            playerList: [],
            totalItem: 0
        };
    }
}
