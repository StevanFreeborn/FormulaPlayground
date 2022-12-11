import { nodeResolve } from '@rollup/plugin-node-resolve';
import injectProcessEnv from 'rollup-plugin-inject-process-env';
import dotenv from 'dotenv';
dotenv.config();

export default [
  {
    input: './scripts/index.js',
    output: {
      file: './scripts/bundled/index.js',
      format: 'iife',
    },
    plugins: [
      nodeResolve(),
      injectProcessEnv(process.env),
    ],
  },
];
