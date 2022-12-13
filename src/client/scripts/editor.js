import { EditorView, basicSetup } from 'codemirror';
import { Decoration, keymap, MatchDecorator, ViewPlugin } from '@codemirror/view';
import { EditorState, Compartment } from '@codemirror/state';
import { javascript } from '@codemirror/lang-javascript';
import { indentWithTab } from '@codemirror/commands';
import { fieldViewPlugin } from './viewPlugins/fieldViewPlugin';
import { listViewPlugin } from './viewPlugins/listViewPlugin';
import { customFunctionViewPlugin } from './viewPlugins/customFunctionsViewPlugin';

export default class Editor {
  static setup = element => {
    const tabSize = new Compartment();

    const editorState = EditorState.create({
      doc: '',
      extensions: [
        basicSetup,
        keymap.of([indentWithTab]),
        javascript(),
        fieldViewPlugin,
        listViewPlugin,
        customFunctionViewPlugin,
        tabSize.of(EditorState.tabSize.of(4)),
      ],
    });

    return new EditorView({
      state: editorState,
      parent: element,
    });
  };
}
