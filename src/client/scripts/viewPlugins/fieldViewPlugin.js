import { Decoration, MatchDecorator, ViewPlugin } from '@codemirror/view';

const fieldDecoration = Decoration.mark({ class: 'field-token' });

const fieldDecorator = new MatchDecorator({
  regexp: /{:.+}/g,
  decoration: m => fieldDecoration,
});

export const fieldViewPlugin = ViewPlugin.define(
  view => ({
    decorations: fieldDecorator.createDeco(view),
    update(u) {
      this.decorations = fieldDecorator.updateDeco(u, this.decorations);
    },
  }),
  {
    decorations: v => v.decorations,
  }
);
