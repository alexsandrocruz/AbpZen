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
  import { useCreateArtist, useUpdateArtist } from "@/lib/abp/hooks/useArtists";
import { toast } from "sonner";

const formSchema = z.object({
  
    name: z.any(),
  
    type: z.any(),
  
    biography: z.any(),
  
    photoUrl: z.any(),
  
    isActive: z.any(),
  
    instagramHandle: z.any(),
  
    websiteUrl: z.any(),
  
    logoUrl: z.any(),
  
    bannerUrl: z.any(),
  
    hexColor: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface ArtistFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function ArtistForm({
  isOpen,
  onClose,
  initialValues,
}: ArtistFormProps) {
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

  const createMutation = useCreateArtist();
const updateMutation = useUpdateArtist();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("Artist updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("Artist created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save artist:", error);
    toast.error(error.message || "Failed to save artist");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit Artist": "Create Artist" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the artist." : "Fill in the details to create a new artist." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="name" > Name * </Label>

<Input id="name" {...register("name") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="type" > Type * </Label>

<Input id="type" {...register("type") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="biography" > Biography</Label>

<Input id="biography" {...register("biography") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="photoUrl" > PhotoUrl</Label>

<Input id="photoUrl" {...register("photoUrl") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="isActive" > IsActive</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="isActive"
checked = { watch("isActive") }
onCheckedChange = {(checked) => setValue("isActive", !!checked)}
              />
  < label htmlFor = "isActive" className = "text-sm font-normal" >
    { watch("isActive") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="instagramHandle" > InstagramHandle</Label>

<Input id="instagramHandle" {...register("instagramHandle") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="websiteUrl" > WebsiteUrl</Label>

<Input id="websiteUrl" {...register("websiteUrl") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="logoUrl" > LogoUrl</Label>

<Input id="logoUrl" {...register("logoUrl") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="bannerUrl" > BannerUrl</Label>

<Input id="bannerUrl" {...register("bannerUrl") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="hexColor" > HexColor</Label>

<Input id="hexColor" {...register("hexColor") } />

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
