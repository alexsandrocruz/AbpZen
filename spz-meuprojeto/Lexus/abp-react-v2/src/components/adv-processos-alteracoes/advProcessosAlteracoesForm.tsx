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
  import { useCreateAdvProcessosAlteracoes, useUpdateAdvProcessosAlteracoes } from "@/lib/abp/hooks/useAdvProcessosAlteracoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idProcessoAlteracao: z.any(),
  
    idProcesso: z.any(),
  
    idUsuario: z.any(),
  
    tsInclusao: z.any(),
  
    texto: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvProcessosAlteracoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvProcessosAlteracoesForm({
  isOpen,
  onClose,
  initialValues,
}: AdvProcessosAlteracoesFormProps) {
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

  const createMutation = useCreateAdvProcessosAlteracoes();
const updateMutation = useUpdateAdvProcessosAlteracoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advProcessosAlteracoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advProcessosAlteracoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprocessosalteracoes:", error);
    toast.error(error.message || "Failed to save advprocessosalteracoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advProcessosAlteracoes": "Create advProcessosAlteracoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprocessosalteracoes." : "Fill in the details to create a new advprocessosalteracoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idProcessoAlteracao" > idProcessoAlteracao</Label>

<Input id="idProcessoAlteracao" type = "number" step = "any" {...register("idProcessoAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso * </Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario * </Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="texto" > texto</Label>

<Input id="texto" {...register("texto") } />

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
