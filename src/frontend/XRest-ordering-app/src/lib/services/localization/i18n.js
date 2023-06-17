import { init, addMessages, getLocaleFromNavigator, _, locale } from 'svelte-i18n'
import { getSelectedLang } from '../../helpers'

for (const lang in langData) {
    addMessages(langData[lang].code, langData[lang].data)
}

init({
    fallbackLocale: 'pl',
    initialLocale: getLocaleFromNavigator(),
});

locale.set(getSelectedLang())

export { locale, _ }