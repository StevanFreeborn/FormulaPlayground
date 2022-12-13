import AppService from '../services/appService.js';
import FieldService from '../services/fieldService.js';

export default class IndexEventHandler {
  constructor(indexState) {
    this.state = indexState;
  }

  handleApiInputChange = async e => {
    this.state.resetRecordInput();
    this.state.resetAppInput();

    if (!e.currentTarget.value) {
      return;
    }

    const res = await AppService.getApps(e.currentTarget.value);

    if (res.ok == false) {
      const data = await res.json();
      const errorMessage = data?.error ?? 'Unable to retrieve apps.';
      this.state.showApiKeyError(errorMessage);
      return;
    }

    const apps = await res.json();

    this.state.addAppOptions(apps);
    this.state.showAppInput();
  };

  handleApiInput = e => {
    this.state.resetApiKeyError();
    this.state.resetAppError();
  };

  handleAppInputChange = async e => {
    this.state.resetAppError();
    this.state.clearFieldsList();

    if (!e.currentTarget.value) {
      this.state.hideRecordInput();
      this.state.resetRecordInput();
      this.state.addFieldsListPlaceHolder();
      return;
    }

    const response = await FieldService.getFields(
      this.state.apiKeyInput.value,
      e.currentTarget.value
    );

    if (response.ok == false) {
      const data = await response.json();
      const errorMessage = data?.error ?? 'Unable to retrieve fields list.';
      this.state.showAppError(errorMessage);
      this.state.hideRecordInput();
      this.state.resetRecordInput();
      this.state.addFieldsListPlaceHolder();
      return;
    }

    this.state.showRecordInput();

    const fields = await response.json();

    this.state.addFieldListItems(fields);
  };

  handleFieldsButtonClick = e => {
    if (this.state.isFieldsModalDisplayed()) {
      this.state.hideFieldsModal();
      return;
    }

    this.state.showFieldsModal();
  };

  handleOperatorsButtonClick = e => {
    if (this.state.isOperatorsModalDisplayed()) {
      this.state.hideOperatorsModal();
      return;
    }

    this.state.showOperatorsModal();
  };

  handleFieldsSearchBoxInput = e => {
    const filterValue = e.currentTarget.value.toLowerCase();
    this.state.filterFieldsList(filterValue);
  };

  handleFieldsListClick = e => {
    if (e.target.id == 'fieldsPlaceHolder') {
      return;
    }

    this.state.insertFieldToken(e.target.innerText);
    this.state.hideFieldsModal();
    this.state.focusOnEditor();
  };

  handleOperatorListClick = e => {
    const symbol = e.target.closest('li').getAttribute('data-operator-symbol');
    this.state.insertOperator(symbol);
    this.state.hideOperatorsModal();
    this.state.focusOnEditor();
  };

  handleDocumentClick = e => {
    const isNotFldModalFldButtonOrAChild =
      e.target != this.state.fieldsModal &&
      !this.state.fieldsModal.contains(e.target) &&
      !this.state.fieldsButton.contains(e.target) &&
      e.target != this.state.fieldsButton;

    const isNotOpModalOpButtonOrAChild =
      e.target != this.state.operatorsModal &&
      !this.state.operatorsModal.contains(e.target) &&
      !this.state.operatorsButton.contains(e.target) &&
      e.target != this.state.operatorsButton;

    if (isNotFldModalFldButtonOrAChild && this.state.isFieldsModalDisplayed()) {
      this.state.hideFieldsModal();
    }

    if (
      isNotOpModalOpButtonOrAChild &&
      this.state.isOperatorsModalDisplayed()
    ) {
      this.state.hideOperatorsModal();
    }
  };
}
