import ClientResourceManager from '@jsroot/lib/clientResourceManager';
import { resource_en } from './resource.en';
import { resource_cs } from './resource.cs';
import { resource_vn } from './resource.vn';

ClientResourceManager.register('en', resource_en);
ClientResourceManager.register('cs', resource_cs);
ClientResourceManager.register('vn', resource_vn);