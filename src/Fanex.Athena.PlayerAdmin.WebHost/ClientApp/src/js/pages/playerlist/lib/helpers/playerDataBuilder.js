export const buildSelectedPlayer = (dataItem) => {
    const { IbcCustId, UserName, Currency, IsDeposit, IsArchived, LicenseeCustId, CurrencyId, CreatedDate } = dataItem;

    return {
        ibcCustId: IbcCustId,
        licenseeCustId: LicenseeCustId,
        userName: UserName,
        currency: Currency,
        isDeposit: IsDeposit,
        isArchived: IsArchived,
        currencyId: CurrencyId,
        createdDate: CreatedDate,
        commissionGroup: dataItem.CommissionGroup,
        customerClass: dataItem.CustomerClass,
        isClosed: dataItem.Status == 2
    };
}

export const  buildLapPlayerList = (playerList) => {
    if (!playerList) {
        return [];
    }

    return playerList.map(item => {
        const {
            CustId,
            UserName,
            CurrentBalance,
            Currency,
            Status,
            RealName,
            email,
            isOnline,
            IBCCustId,
            isArchived,
            LastTransaction,
            CurrencyCode,
            OutBalance,
            IBCUserName,
            Vender_Member_ID,
            CreatedDateString
        } = item;

        return {
            IbcCustId: IBCCustId,
            LicenseeCustId: CustId,
            UserName,
            SystemId: IBCUserName,
            VendorMemberId: Vender_Member_ID,
            Currency: CurrencyCode,
            CurrentBalance,
            OutstandingBalance: OutBalance,
            Status,
            RealName,
            LastTransaction,
            Email: email,
            IsOnline: isOnline,
            IsDeposit: IBCCustId > 0,
            IsArchived: !!isArchived,
            CurrencyId: Currency,
            CreatedDate: CreatedDateString,
        };
    });
}

export const buildLrfPlayerList = (playerList) => {
    if (!playerList) {
        return [];
    }

    return playerList.map(item => {
        const {
            IbcCustId,
            UserName,
            SystemId,
            Currency,
            CurrentBalance,
            OutstandingBalance,
            CurrencyId,
            OddsGroup,
            BaStatus,
            SpecialLevel,
            CommissionGroup,
            CustomerClass,
            Status
        } = item;

        return {
            IbcCustId,
            LicenseeCustId: 0, // no sync from Licensee DB
            UserName,
            SystemId,
            Currency,
            CurrentBalance,
            OutstandingBalance,
            IsDeposit: IbcCustId > 0, // always true
            CurrencyId,
            OddsGroup,
            BaStatus,
            SpecialLevel,
            CommissionGroup,
            CustomerClass,
            Status
        };
    });
}

export const buildCurrentActivePlayer = (dataItem) => {
    const {
        IbcCustId, UserName, Currency, Status, OddsGroup, SpecialLevel,
        IsDeposit, IsArchived, LicenseeCustId, CurrencyId, CreatedDate
    } = dataItem;

    return {
        ibcCustId: IbcCustId,
        licenseeCustId: LicenseeCustId,
        userName: UserName,
        currency: Currency,
        status: Status,
        oddsGroup: OddsGroup,
        specialLevel: SpecialLevel,
        isDeposit: IsDeposit,
        isArchived: IsArchived,
        currencyId: CurrencyId,
        createdDate: CreatedDate,
        commissionGroup: dataItem.CommissionGroup,
        customerClass: dataItem.CustomerClass,
        isClosed: dataItem.Status == 2
    };
}