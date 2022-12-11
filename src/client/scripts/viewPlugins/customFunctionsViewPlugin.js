import { Decoration, MatchDecorator, ViewPlugin } from '@codemirror/view';
import { CUSTOM_FUNCTIONS } from '../constants';

const customFunctionDecoration = Decoration.mark({ class: 'custom-function' });

const customFunctionRegex = new RegExp(CUSTOM_FUNCTIONS.join('|'), 'g');

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