<script>
import { locale } from 'svelte-i18n'
import { saveSelectedLang} from '../helpers'

async function changeLang(code){
    saveSelectedLang(code)
    locale.set(code)
}

</script>
<div class="text-end flags-panel">
    <!-- svelte-ignore missing-declaration -->
    {#each langData as lang }
        <!-- svelte-ignore a11y-invalid-attribute -->
        <a href="#" on:click={()=> changeLang(lang.code)} ><img src="/assets/img/{lang.icon}" alt="{lang.title}" title="{lang.title}"  class="flag-icon" /></a>
    {/each}
</div>
<style>
.flag-icon { 
    width: 40px;
}
.flags-panel a{
    margin-right: 15px;
}
</style>