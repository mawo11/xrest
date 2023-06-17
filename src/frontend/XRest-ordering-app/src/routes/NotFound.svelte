<script>
    import { _ } from '../lib/services/localization/i18n'
    import LayoutSimple from './_LayoutSimple.svelte'
</script>

<LayoutSimple>
    <div class="container-fluid">
        <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo" />
            </div>
        </div>
        
        <div class="mx-auto card my-5 col-md-11 col-xxl-7 text-center">
            
            <div class="card-body">
                <img
                src="assets/img/404.png"
                class=""
                alt="logo"
            />
            </div>
        </div>
    </div>
</LayoutSimple>
<style>
.card-body img {
    margin: 0;
}
</style>
