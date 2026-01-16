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
  import { useCreateFinPrestacaoContas, useUpdateFinPrestacaoContas } from "@/lib/abp/hooks/useFinPrestacaoContases";
import { toast } from "sonner";

const formSchema = z.object({
  
    idPrestacao: z.any(),
  
    idLancamento: z.any(),
  
    levantado: z.any(),
  
    irpj: z.any(),
  
    carta: z.any(),
  
    honorarios: z.any(),
  
    tarifa: z.any(),
  
    liquidoRecebido: z.any(),
  
    tsInclusao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinPrestacaoContasFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinPrestacaoContasForm({
  isOpen,
  onClose,
  initialValues,
}: FinPrestacaoContasFormProps) {
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

  const createMutation = useCreateFinPrestacaoContas();
const updateMutation = useUpdateFinPrestacaoContas();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finPrestacaoContas updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finPrestacaoContas created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finprestacaocontas:", error);
    toast.error(error.message || "Failed to save finprestacaocontas");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finPrestacaoContas": "Create finPrestacaoContas" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finprestacaocontas." : "Fill in the details to create a new finprestacaocontas." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idPrestacao" > idPrestacao</Label>

<Input id="idPrestacao" type = "number" step = "any" {...register("idPrestacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idLancamento" > idLancamento * </Label>

<Input id="idLancamento" type = "number" step = "any" {...register("idLancamento") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="levantado" > levantado</Label>

<Input id="levantado" type = "number" step = "any" {...register("levantado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="irpj" > irpj</Label>

<Input id="irpj" type = "number" step = "any" {...register("irpj") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="carta" > carta</Label>

<Input id="carta" type = "number" step = "any" {...register("carta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="honorarios" > honorarios</Label>

<Input id="honorarios" type = "number" step = "any" {...register("honorarios") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tarifa" > tarifa</Label>

<Input id="tarifa" type = "number" step = "any" {...register("tarifa") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="liquidoRecebido" > liquidoRecebido</Label>

<Input id="liquidoRecebido" type = "number" step = "any" {...register("liquidoRecebido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

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
