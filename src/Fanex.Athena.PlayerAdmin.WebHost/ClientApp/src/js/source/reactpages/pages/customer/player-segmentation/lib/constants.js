import React from 'react';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';

export const tabKeys = {
    problemPlayer: 'ProblemPlayer',
    vipPlayerManagement: 'VipPlayerManagement'
}

const dynamicImports = {
    [tabKeys.problemPlayer]: () => import(/* webpackChunkName: "customer/playersegmentation/problemPlayer" */ '../tabs/problem-player/index'),
    [tabKeys.vipPlayerManagement]: () => import(/* webpackChunkName: "customer/playersegmentation/vipPlayerManagement" */ '../tabs/vip-player-management/index')
}

export const tabMenuList = AppData.TabKeys.map(_ => {
    return {
        key: _, name: LangHelper.getSingleResource(_), component: React.lazy(dynamicImports[_]), preLoad: dynamicImports[_]
    }
})

export const preLoadComponents = (lazyComponents) => {
    lazyComponents && setTimeout(() => lazyComponents.forEach(_ => _.preLoad && _.preLoad()), 0)
}

export const SortMapper = {
    sortCol: {
        'systemId': 1,
        'externalId': 2,
        'markDate': 3,
        'customerClass': 4
    },
    sortType: {
        'asc': 1,
        'desc': 2
    }
}

export const ViewType = {
    Normal: 0,
    Susidiary: 1,
    WhiteLabel: 2
}

export const TreeViewOption = {
    childrenField: 'Children',
    keyField: 'Value',
    valueField: 'Name',
    statusField: 'Checked',
    indent: 30,
    includeSelectedParentKey: false,
    useOriginalDataSource: false
}

export const UpdateVIPStatusCode = {
    Success: 0,
    InvalidSystemId: 100,
    NoChangeBySystemId: 101,
    Avoid: 102,
    InvalidExternalId: 103,
    NoChangeByExternalId: 104,
    NoChangeByBAStatus: 105
}

export const errorResponse = {
    data: null,
    isSuccess: false
}

export const successResponse = {
    isSuccess: true
}