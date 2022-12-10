import { EditorView, basicSetup } from 'codemirror';
import { keymap }  from '@codemirror/view';
import { EditorState, Compartment } from '@codemirror/state';
import { javascript } from '@codemirror/lang-javascript';
import { indentWithTab } from '@codemirror/commands';

export default class Editor {
  static setup = state => {
    const language = new Compartment();
    const tabSize = new Compartment();

    const editorState = EditorState.create({
      doc: '',
      extensions: [
        basicSetup,
        keymap.of([indentWithTab]),
        language.of(javascript()),
        tabSize.of(EditorState.tabSize.of(4)),
      ],
    });

    return new EditorView({
      state: editorState,
      parent: state.editor,
    });
  };
}
