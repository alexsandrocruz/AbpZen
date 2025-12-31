export enum ToolbarTranslateKeys {
  ContextMenuWelcome = 'toolbar.contextMenu.Welcome',
}
export type ToolbarTranslateValues = {
  [key in ToolbarTranslateKeys]: string;
};
export const ToolbarTranslateDefaults: ToolbarTranslateValues = {
  [ToolbarTranslateKeys.ContextMenuWelcome]: 'Welcome',
};
