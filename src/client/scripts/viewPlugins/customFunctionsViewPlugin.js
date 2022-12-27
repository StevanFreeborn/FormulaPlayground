import { Decoration, MatchDecorator, ViewPlugin } from '@codemirror/view';
import { CUSTOM_FUNCTIONS } from '../constants';

const customFunctionDecoration = Decoration.mark({ class: 'custom-function' });

const customFunctionNames = CUSTOM_FUNCTIONS
.map(fn => fn.name)
.sort((a,b) => b.length - a.length)
.join('|');

const customFunctionRegex = new RegExp(customFunctionNames, 'g');

const customFunctionDecorator = new MatchDecorator({
  regexp: customFunctionRegex,
  decoration: m => customFunctionDecoration
});

export const customFunctionViewPlugin = ViewPlugin.define(
  view => ({
    decorations: customFunctionDecorator.createDeco(view),
    update(u) {
      this.decorations = customFunctionDecorator.updateDeco(
        u,
        this.decorations
      );
    },
  }),
  {
    decorations: v => v.decorations,
  }
);