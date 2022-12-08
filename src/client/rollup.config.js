import { nodeResolve } from '@rollup/plugin-node-resolve';

export default {
  input: './scripts/editor.js',
  output: {
    file: './scripts/bundled/editor.js',
    format: 'iife',
  },
  plugins: [nodeResolve()],
};
