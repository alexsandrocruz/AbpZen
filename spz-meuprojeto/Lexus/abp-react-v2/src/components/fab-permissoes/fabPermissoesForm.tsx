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
  import { useCreateFabPermissoes, useUpdateFabPermissoes } from "@/lib/abp/hooks/useFabPermissoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idPermissao: z.any(),
  
    idPermissaoTipo: z.any(),
  
    modulo: z.any(),
  
    descricao: z.any(),
  
    varSession: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabPermissoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabPermissoesForm({
  isOpen,
  onClose,
  initialValues,
}: FabPermissoesFormProps) {
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

  const createMutation = useCreateFabPermissoes();
const updateMutation = useUpdateFabPermissoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabPermissoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabPermissoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fabpermissoes:", error);
    toast.error(error.message || "Failed to save fabpermissoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabPermissoes": "Create fabPermissoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fabpermissoes." : "Fill in the details to create a new fabpermissoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idPermissao" > idPermissao</Label>

<Input id="idPermissao" type = "number" step = "any" {...register("idPermissao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idPermissaoTipo" > idPermissaoTipo * </Label>

<Input id="idPermissaoTipo" type = "number" step = "any" {...register("idPermissaoTipo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="modulo" > modulo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="modulo"
checked = { watch("modulo") }
onCheckedChange = {(checked) => setValue("modulo", !!checked)}
              />
  < label htmlFor = "modulo" className = "text-sm font-normal" >
    { watch("modulo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="descricao" > descricao</Label>

<Input id="descricao" {...register("descricao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="varSession" > varSession</Label>

<Input id="varSession" {...register("varSession") } />

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
