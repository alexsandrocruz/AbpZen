import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAutoFTPsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAutoFTPs(input: GetAutoFTPsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["autoFTPs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/auto-ftp", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAutoFTPs() {
  return useQuery({
    queryKey: ["autoFTPs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/auto-ftp", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAutoFTP(id: string) {
  return useQuery({
    queryKey: ["autoFTP", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/auto-ftp/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAutoFTP() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/auto-ftp", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["autoFTPs"] });
    },
  });
}

export function useUpdateAutoFTP() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/auto-ftp/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["autoFTPs"] });
      queryClient.invalidateQueries({ queryKey: ["autoFTP", data.id] });
    },
  });
}

export function useDeleteAutoFTP() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/auto-ftp/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["autoFTPs"] });
    },
  });
}


