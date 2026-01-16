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
  import { useCreateFinContas, useUpdateFinContas } from "@/lib/abp/hooks/useFinContases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idConta: z.any(),
  
    titulo: z.any(),
  
    banco: z.any(),
  
    agencia: z.any(),
  
    conta: z.any(),
  
    favorecido: z.any(),
  
    limite: z.any(),
  
    padraoFluxo: z.any(),
  
    considerarIndicador: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    saldoInicial: z.any(),
  
    padrao: z.any(),
  
    codigo: z.any(),
  
    cor: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinContasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinContasForm({
  isOpen,
  onClose,
  initialValues,
}: FinContasFormProps) {
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

  const createMutation = useCreateFinContas();
const updateMutation = useUpdateFinContas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finContas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finContas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fincontas:", error);
    toast.error(error.message || "Failed to save fincontas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finContas": "Create finContas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fincontas." : "Fill in the details to create a new fincontas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idConta" > idConta</Label>

<Input id="idConta" type = "number" step = "any" {...register("idConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="banco" > banco</Label>

<Input id="banco" {...register("banco") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="agencia" > agencia</Label>

<Input id="agencia" {...register("agencia") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="conta" > conta</Label>

<Input id="conta" {...register("conta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="favorecido" > favorecido</Label>

<Input id="favorecido" {...register("favorecido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="limite" > limite</Label>

<Input id="limite" type = "number" step = "any" {...register("limite") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="padraoFluxo" > padraoFluxo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="padraoFluxo"
checked = { watch("padraoFluxo") }
onCheckedChange = {(checked) => setValue("padraoFluxo", !!checked)}
              />
  < label htmlFor = "padraoFluxo" className = "text-sm font-normal" >
    { watch("padraoFluxo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="considerarIndicador" > considerarIndicador</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="considerarIndicador"
checked = { watch("considerarIndicador") }
onCheckedChange = {(checked) => setValue("considerarIndicador", !!checked)}
              />
  < label htmlFor = "considerarIndicador" className = "text-sm font-normal" >
    { watch("considerarIndicador") ?"Enabled": "Disabled" }
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
  <Label htmlFor="saldoInicial" > saldoInicial</Label>

<Input id="saldoInicial" type = "number" step = "any" {...register("saldoInicial") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="padrao" > padrao</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="padrao"
checked = { watch("padrao") }
onCheckedChange = {(checked) => setValue("padrao", !!checked)}
              />
  < label htmlFor = "padrao" className = "text-sm font-normal" >
    { watch("padrao") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="codigo" > codigo</Label>

<Input id="codigo" {...register("codigo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="cor" > cor</Label>

<Input id="cor" {...register("cor") } />

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
