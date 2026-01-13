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
  import { useCreateFinPlanoContasDet, useUpdateFinPlanoContasDet } from "@/lib/abp/hooks/useFinPlanoContasDets";
import { toast } from "sonner";

const formSchema = z.object({
  
    idPlanoContasDet: z.any(),
  
    idPlanoConta: z.any(),
  
    titulo: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FinPlanoContasDetFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FinPlanoContasDetForm({
  isOpen,
  onClose,
  initialValues,
}: FinPlanoContasDetFormProps) {
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

  const createMutation = useCreateFinPlanoContasDet();
const updateMutation = useUpdateFinPlanoContasDet();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("finPlanoContasDet updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("finPlanoContasDet created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save finplanocontasdet:", error);
    toast.error(error.message || "Failed to save finplanocontasdet");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit finPlanoContasDet": "Create finPlanoContasDet" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the finplanocontasdet." : "Fill in the details to create a new finplanocontasdet." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idPlanoContasDet" > idPlanoContasDet</Label>

<Input id="idPlanoContasDet" type = "number" step = "any" {...register("idPlanoContasDet") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPlanoConta" > idPlanoConta * </Label>

<Input id="idPlanoConta" type = "number" step = "any" {...register("idPlanoConta") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

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
