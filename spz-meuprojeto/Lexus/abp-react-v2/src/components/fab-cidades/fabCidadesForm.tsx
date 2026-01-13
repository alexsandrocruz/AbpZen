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
  import { useCreateFabCidades, useUpdateFabCidades } from "@/lib/abp/hooks/useFabCidadeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idCidade: z.any(),
  
    descricao: z.any(),
  
    codigoIBGE: z.any(),
  
    idEstado: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabCidadesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabCidadesForm({
  isOpen,
  onClose,
  initialValues,
}: FabCidadesFormProps) {
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

  const createMutation = useCreateFabCidades();
const updateMutation = useUpdateFabCidades();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabCidades updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabCidades created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabcidades:", error);
    toast.error(error.message || "Failed to save fabcidades");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabCidades": "Create fabCidades" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabcidades." : "Fill in the details to create a new fabcidades." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idCidade" > idCidade</Label>

<Input id="idCidade" type = "number" step = "any" {...register("idCidade") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="codigoIBGE" > codigoIBGE</Label>

<Input id="codigoIBGE" {...register("codigoIBGE") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idEstado" > idEstado</Label>

<Input id="idEstado" type = "number" step = "any" {...register("idEstado") } />

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
