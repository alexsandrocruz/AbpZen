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
  import { useCreateFlwConfigExcecoes, useUpdateFlwConfigExcecoes } from "@/lib/abp/hooks/useFlwConfigExcecoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idConfig: z.any(),
  
    tipoMarcacoes: z.any(),
  
    idHistoricoTipo: z.any(),
  
    data: z.any(),
  
    qtde: z.any(),
  
    manhaQtde: z.any(),
  
    tardeQtde: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FlwConfigExcecoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FlwConfigExcecoesForm({
  isOpen,
  onClose,
  initialValues,
}: FlwConfigExcecoesFormProps) {
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

  const createMutation = useCreateFlwConfigExcecoes();
const updateMutation = useUpdateFlwConfigExcecoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("flwConfigExcecoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("flwConfigExcecoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save flwconfigexcecoes:", error);
    toast.error(error.message || "Failed to save flwconfigexcecoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit flwConfigExcecoes": "Create flwConfigExcecoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the flwconfigexcecoes." : "Fill in the details to create a new flwconfigexcecoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idConfig" > idConfig</Label>

<Input id="idConfig" type = "number" step = "any" {...register("idConfig") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tipoMarcacoes" > tipoMarcacoes</Label>

<Input id="tipoMarcacoes" {...register("tipoMarcacoes") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idHistoricoTipo" > idHistoricoTipo * </Label>

<Input id="idHistoricoTipo" type = "number" step = "any" {...register("idHistoricoTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="data" > data</Label>

<Input id="data" {...register("data") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="qtde" > qtde</Label>

<Input id="qtde" type = "number" step = "any" {...register("qtde") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="manhaQtde" > manhaQtde</Label>

<Input id="manhaQtde" type = "number" step = "any" {...register("manhaQtde") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tardeQtde" > tardeQtde</Label>

<Input id="tardeQtde" type = "number" step = "any" {...register("tardeQtde") } />

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
