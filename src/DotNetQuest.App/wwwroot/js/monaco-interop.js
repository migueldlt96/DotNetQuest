// Monaco Editor Interop for DotNet Quest
window.monacoInterop = {
    editors: {},

    initialize: async function(elementId, initialCode, dotNetHelper) {
        // Configure Monaco loader
        require.config({
            paths: {
                'vs': 'https://cdnjs.cloudflare.com/ajax/libs/monaco-editor/0.45.0/min/vs'
            }
        });

        return new Promise((resolve) => {
            require(['vs/editor/editor.main'], function() {
                // Define custom theme matching the game's dark theme
                monaco.editor.defineTheme('dotnetquest', {
                    base: 'vs-dark',
                    inherit: true,
                    rules: [
                        { token: 'comment', foreground: '6A9955' },
                        { token: 'keyword', foreground: 'C586C0' },
                        { token: 'string', foreground: 'CE9178' },
                        { token: 'number', foreground: 'B5CEA8' },
                        { token: 'type', foreground: '4EC9B0' },
                        { token: 'class', foreground: '4EC9B0' },
                        { token: 'function', foreground: 'DCDCAA' },
                        { token: 'variable', foreground: '9CDCFE' },
                    ],
                    colors: {
                        'editor.background': '#0a0a15',
                        'editor.foreground': '#00ff88',
                        'editorCursor.foreground': '#00ff88',
                        'editor.lineHighlightBackground': '#1a1a2e',
                        'editorLineNumber.foreground': '#555577',
                        'editor.selectionBackground': '#264f78',
                        'editor.inactiveSelectionBackground': '#3a3d41'
                    }
                });

                const container = document.getElementById(elementId);
                if (!container) {
                    console.error('Monaco container not found:', elementId);
                    resolve(false);
                    return;
                }

                const editor = monaco.editor.create(container, {
                    value: initialCode || '',
                    language: 'csharp',
                    theme: 'dotnetquest',
                    fontSize: 14,
                    fontFamily: "'Cascadia Code', 'Fira Code', 'Consolas', monospace",
                    minimap: { enabled: false },
                    scrollBeyondLastLine: false,
                    automaticLayout: true,
                    lineNumbers: 'on',
                    renderLineHighlight: 'line',
                    tabSize: 4,
                    insertSpaces: true,
                    wordWrap: 'on',
                    suggestOnTriggerCharacters: true,
                    quickSuggestions: true,
                    parameterHints: { enabled: true },
                    autoClosingBrackets: 'always',
                    autoClosingQuotes: 'always',
                    formatOnPaste: true,
                    formatOnType: true
                });

                // Store editor reference
                window.monacoInterop.editors[elementId] = {
                    editor: editor,
                    dotNetHelper: dotNetHelper
                };

                // Notify .NET when content changes
                editor.onDidChangeModelContent(() => {
                    const code = editor.getValue();
                    if (dotNetHelper) {
                        dotNetHelper.invokeMethodAsync('HandleCodeChangedFromJs', code);
                    }
                });

                // Add keyboard shortcut for running code (Ctrl/Cmd + Enter)
                editor.addCommand(monaco.KeyMod.CtrlCmd | monaco.KeyCode.Enter, () => {
                    if (dotNetHelper) {
                        dotNetHelper.invokeMethodAsync('HandleRunCodeFromJs');
                    }
                });

                resolve(true);
            });
        });
    },

    getValue: function(elementId) {
        const editorData = window.monacoInterop.editors[elementId];
        if (editorData && editorData.editor) {
            return editorData.editor.getValue();
        }
        return '';
    },

    setValue: function(elementId, value) {
        const editorData = window.monacoInterop.editors[elementId];
        if (editorData && editorData.editor) {
            editorData.editor.setValue(value || '');
        }
    },

    setMarkers: function(elementId, markers) {
        const editorData = window.monacoInterop.editors[elementId];
        if (editorData && editorData.editor) {
            const model = editorData.editor.getModel();
            if (model) {
                const monacoMarkers = markers.map(m => ({
                    severity: m.severity === 'error' ? monaco.MarkerSeverity.Error : monaco.MarkerSeverity.Warning,
                    startLineNumber: m.startLine,
                    startColumn: m.startColumn,
                    endLineNumber: m.endLine || m.startLine,
                    endColumn: m.endColumn || m.startColumn + 1,
                    message: m.message
                }));
                monaco.editor.setModelMarkers(model, 'dotnetquest', monacoMarkers);
            }
        }
    },

    clearMarkers: function(elementId) {
        const editorData = window.monacoInterop.editors[elementId];
        if (editorData && editorData.editor) {
            const model = editorData.editor.getModel();
            if (model) {
                monaco.editor.setModelMarkers(model, 'dotnetquest', []);
            }
        }
    },

    focus: function(elementId) {
        const editorData = window.monacoInterop.editors[elementId];
        if (editorData && editorData.editor) {
            editorData.editor.focus();
        }
    },

    dispose: function(elementId) {
        const editorData = window.monacoInterop.editors[elementId];
        if (editorData && editorData.editor) {
            editorData.editor.dispose();
            delete window.monacoInterop.editors[elementId];
        }
    }
};
