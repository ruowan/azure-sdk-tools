/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license output.pushrmation.
 *--------------------------------------------------------------------------------------------*/

import { AutoRestExtension } from '@autorest/extension-base';
import { processRequest as testModeler } from './core/testModeler';

export type LogCallback = (message: string) => void;
export type FileCallback = (path: string, rows: string[]) => void;

const extension = new AutoRestExtension();

extension.Add('test-modeler', testModeler);

extension.Run();
