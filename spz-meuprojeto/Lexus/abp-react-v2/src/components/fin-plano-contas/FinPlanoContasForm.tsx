import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateFinPlanoContas, useUpdateFinPlanoContas } from "@/lib/abp/hooks/useFinPlanoContases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idPlanoConta: z.any(),
  
    idGrupo: z.any(),
  
    titulo: z.any(),
  
    codigo: z.any(),
  
    pagamentoSempreLiberado: z.any(),
  
    permiteLancamentoQuitado: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    padraoVendas: z.any(),
  
    antecipaVencimento: z.any(),
  
    terceiroNivel: z.any(),
  
    criarPeloFinanceiro: z.any(),
  
    valoresRestritos: z.any(),
  
    naoAbatePagtoDoSaldoDoCliente: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinPlanoContasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinPlanoContasForm({
  isOpen,
  onClose,
  initialValues,
}: FinPlanoContasFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateFinPlanoContas();
const updateMutation = useUpdateFinPlanoContas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finPlanoContas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finPlanoContas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finplanocontas:", error);
    toast.error(error.message || "Failed to save finplanocontas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finPlanoContas": "Create finPlanoContas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finplanocontas." : "Fill in the details to create a new finplanocontas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idPlanoConta" > idPlanoConta</Label>

<Input id="idPlanoConta" type = "number" step = "any" {...register("idPlanoConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idGrupo" > idGrupo * </Label>

<Input id="idGrupo" type = "number" step = "any" {...register("idGrupo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="codigo" > codigo</Label>

<Input id="codigo" {...register("codigo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="pagamentoSempreLiberado" > pagamentoSempreLiberado</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="pagamentoSempreLiberado"
checked = { watch("pagamentoSempreLiberado") }
onCheckedChange = {(checked) => setValue("pagamentoSempreLiberado", !!checked)}
              />
  < label htmlFor = "pagamentoSempreLiberado" className = "text-sm font-normal" >
    { watch("pagamentoSempreLiberado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="permiteLancamentoQuitado" > permiteLancamentoQuitado</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="permiteLancamentoQuitado"
checked = { watch("permiteLancamentoQuitado") }
onCheckedChange = {(checked) => setValue("permiteLancamentoQuitado", !!checked)}
              />
  < label htmlFor = "permiteLancamentoQuitado" className = "text-sm font-normal" >
    { watch("permiteLancamentoQuitado") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="padraoVendas" > padraoVendas</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="padraoVendas"
checked = { watch("padraoVendas") }
onCheckedChange = {(checked) => setValue("padraoVendas", !!checked)}
              />
  < label htmlFor = "padraoVendas" className = "text-sm font-normal" >
    { watch("padraoVendas") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="antecipaVencimento" > antecipaVencimento</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="antecipaVencimento"
checked = { watch("antecipaVencimento") }
onCheckedChange = {(checked) => setValue("antecipaVencimento", !!checked)}
              />
  < label htmlFor = "antecipaVencimento" className = "text-sm font-normal" >
    { watch("antecipaVencimento") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="terceiroNivel" > terceiroNivel</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="terceiroNivel"
checked = { watch("terceiroNivel") }
onCheckedChange = {(checked) => setValue("terceiroNivel", !!checked)}
              />
  < label htmlFor = "terceiroNivel" className = "text-sm font-normal" >
    { watch("terceiroNivel") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="criarPeloFinanceiro" > criarPeloFinanceiro</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="criarPeloFinanceiro"
checked = { watch("criarPeloFinanceiro") }
onCheckedChange = {(checked) => setValue("criarPeloFinanceiro", !!checked)}
              />
  < label htmlFor = "criarPeloFinanceiro" className = "text-sm font-normal" >
    { watch("criarPeloFinanceiro") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="valoresRestritos" > valoresRestritos</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="valoresRestritos"
checked = { watch("valoresRestritos") }
onCheckedChange = {(checked) => setValue("valoresRestritos", !!checked)}
              />
  < label htmlFor = "valoresRestritos" className = "text-sm font-normal" >
    { watch("valoresRestritos") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="naoAbatePagtoDoSaldoDoCliente" > naoAbatePagtoDoSaldoDoCliente</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="naoAbatePagtoDoSaldoDoCliente"
checked = { watch("naoAbatePagtoDoSaldoDoCliente") }
onCheckedChange = {(checked) => setValue("naoAbatePagtoDoSaldoDoCliente", !!checked)}
              />
  < label htmlFor = "naoAbatePagtoDoSaldoDoCliente" className = "text-sm font-normal" >
    { watch("naoAbatePagtoDoSaldoDoCliente") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
