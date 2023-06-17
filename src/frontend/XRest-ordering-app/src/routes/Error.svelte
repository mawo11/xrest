<script>
    import LayoutSimple from './_LayoutSimple.svelte'
    import { _ } from '../lib/services/localization/i18n'
    import { push } from '../lib/router'

    async function goStart() {
        push('/')
    }
</script>

<LayoutSimple>
    <div class="container-fluid">
        <div
            class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo"
                />
            </div>
        </div>
        <div class="mx-auto card my-5 col-md-11 col-xxl-7 card text-center">
            <div class="card-body text-center card">
                <h2>{@html $_("error.description")}</h2>
                <br/>
                <button
                    type="input"
                    on:click|preventDefault|stopPropagation={goStart}
                    class="btn kk-color text-uppercase text-white shadow">
                    {$_('error.goStart')}
                </button> 
            </div>
        </div>
    </div>
</LayoutSimple>
