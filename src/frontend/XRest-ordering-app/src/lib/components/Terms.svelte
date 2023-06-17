<script>
    import { _ } from '../services/localization/i18n'
    import { selectedRestaurant } from '../stores'

    function showTerms() {
        const id = $selectedRestaurant.id
        window.open(`/restaurant/${id}/terms`, '_blank')
    }

    function showPolicy() {
        window.open(`/policy`, '_blank')
    }
</script>

{$_("summary.terms.part1")}
<!-- svelte-ignore a11y-invalid-attribute -->
<a href="#" on:click|preventDefault|stopPropagation={showTerms}>{$_("summary.terms.part2")}</a>
{$_("summary.terms.part3")}
<!-- svelte-ignore a11y-invalid-attribute -->
<a href="#" on:click|preventDefault|stopPropagation={showPolicy}>{$_("summary.terms.part4")}</a>