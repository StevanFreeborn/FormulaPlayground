import { Decoration, MatchDecorator, ViewPlugin } from '@codemirror/view';

const listDecoration = Decoration.mark({ class: 'list-value-token' });

const listDecorator = new MatchDecorator({
  regexp: /\[:.+\]/g,
  decoration: m => listDecoration
});

export const listViewPlugin = ViewPlugin.define(
  view => ({
    decorations: listDecorator.createDeco(view),
    update(u) {
      this.decorations = listDecorator.updateDeco(u, this.decorations);
    },
  }),
  {
    decorations: v => v.decorations,
  }
);