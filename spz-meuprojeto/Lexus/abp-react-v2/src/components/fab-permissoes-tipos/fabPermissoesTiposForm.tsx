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
  import { useCreateFabPermissoesTipos, useUpdateFabPermissoesTipos } from "@/lib/abp/hooks/useFabPermissoesTiposes";
import { toast } from "sonner";

const formSchema = z.object({
  
    idPermissaoTipo: z.any(),
  
    descricao: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabPermissoesTiposFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabPermissoesTiposForm({
  isOpen,
  onClose,
  initialValues,
}: FabPermissoesTiposFormProps) {
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

  const createMutation = useCreateFabPermissoesTipos();
const updateMutation = useUpdateFabPermissoesTipos();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabPermissoesTipos updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabPermissoesTipos created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabpermissoestipos:", error);
    toast.error(error.message || "Failed to save fabpermissoestipos");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabPermissoesTipos": "Create fabPermissoesTipos" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabpermissoestipos." : "Fill in the details to create a new fabpermissoestipos." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idPermissaoTipo" > idPermissaoTipo</Label>

<Input id="idPermissaoTipo" type = "number" step = "any" {...register("idPermissaoTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

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
