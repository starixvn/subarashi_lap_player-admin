import ClientResourceManager from '@jsroot/lib/clientResourceManager';
import { resource_en } from './resource.en';
import { resource_cs } from './resource.cs';
import { resource_vn } from './resource.vn';
import { resource_th } from './resource.th';
import { resource_id } from './resource.id';

ClientResourceManager.register('en', resource_en);
ClientResourceManager.register('cs', resource_cs);
ClientResourceManager.register('vn', resource_vn);
ClientResourceManager.register('th', resource_th);
ClientResourceManager.register('id', resource_id);