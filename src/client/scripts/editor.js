import { EditorView, basicSetup } from 'codemirror';
import { EditorState, Compartment } from '@codemirror/state';
import { javascript } from '@codemirror/lang-javascript';

const language = new Compartment();
const tabSize = new Compartment();

const state = EditorState.create({
  doc: '',
  extensions: [
    basicSetup,
    language.of(javascript()),
    tabSize.of(EditorState.tabSize.of(4)),
  ],
})

const editor = new EditorView({
  state: state,
  parent: document.getElementById('editor'),
});