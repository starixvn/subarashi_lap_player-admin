export const bindDataUserGuideWithPermission = (content, hasVIPPermission) => {
    if(!hasVIPPermission) {
        content = bindDataUserGuideWithVIPPermisson(content);
    }
    return content;
} 

const bindDataUserGuideWithVIPPermisson = content => {
    content = dataVIPPlayerBinder.reduce((prev, current) => prev.replace(current.key, current.data), content);
    return content;
}
const dataVIPPlayerBinder =  [{ key: '{onlyVipManagement}',  data: 'only-vip-management' }];